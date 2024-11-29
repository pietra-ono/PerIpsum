using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Enums;
using PerIpsumOriginal.Migrations;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios;
using PerIpsumOriginal.Repositorios.IRepositorios;
using PerIpsumOriginal.ViewModels;
using System.Diagnostics;

namespace PerIpsumOriginal.Controllers
{
    public class AdminController : Controller
    {

        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly IConteudoRascunhoRepositorio _conteudoRascunhoRepositorio;
        private readonly IConteudoAprovarRepositorio _conteudoAprovarRepositorio;
        private readonly IConteudoRepositorio _conteudoRepositorio;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly PerIpsumDbContext _dbContext;

        public AdminController(IContatoRepositorio contatoRepositorio,
            IConteudoRascunhoRepositorio conteudoRascunhoRepositorio,
            IConteudoAprovarRepositorio conteudoAprovarRepositorio,
            IConteudoRepositorio conteudoRepositorio,
            IWebHostEnvironment environment,
            UserManager<UsuarioModel> userManager,
            PerIpsumDbContext dbContext
            )
        {
            _contatoRepositorio = contatoRepositorio;
            _conteudoRascunhoRepositorio = conteudoRascunhoRepositorio;
            _conteudoAprovarRepositorio = conteudoAprovarRepositorio;
            _conteudoRepositorio = conteudoRepositorio;
            _environment = environment;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        // ================================= RASCUNHOS =================================
        [HttpGet]
        [Authorize(Roles = "Admin, Parcerias")]
        public IActionResult Rascunhos()
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado");
            }

            var rascunhos = _conteudoRascunhoRepositorio.ObterRascunhosPorUsuario(usuarioId);
            var viewModel = new RascunhoViewModel
            {
                Rascunhos = rascunhos
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AdicionarRascunho(ConteudoRascunhoModel rascunho, IFormFile imagem, string acao)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado");
            }
            rascunho.UsuarioId = usuarioId;


            if (string.IsNullOrEmpty(acao))
            {
                return RedirectToAction("Rascunhos");
            }
            // Lógica para salvar a imagem
            if (imagem != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");
                string filePath = Guid.NewGuid().ToString() + "_" + imagem.FileName;
                string fullPath = Path.Combine(uploadsFolder, filePath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                rascunho.Imagem = filePath;
            }

            if (!string.IsNullOrWhiteSpace(rascunho.Categorias))
            {
                // Divide as palavras, remove espaços extras
                var categoriasArray = rascunho.Categorias
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Verifica o número de categorias
                if (categoriasArray.Length > 7)
                {
                    ModelState.AddModelError("Categorias", "Você pode adicionar no máximo 7 categorias.");
                    return View(rascunho);
                }

                // Formata as categorias
                rascunho.Categorias = string.Join(", ", categoriasArray) + ".";
            }

            if (acao == "SalvarRascunho")
            {
                // Preencher valores nulos para rascunho
                rascunho.Nome = string.IsNullOrEmpty(rascunho.Nome) ? ValoresNulosModel.Nome : rascunho.Nome;
                rascunho.Descricao = string.IsNullOrEmpty(rascunho.Descricao) ? ValoresNulosModel.Descricao : rascunho.Descricao;
                rascunho.Pais = rascunho.Pais == 0 ? ValoresNulosModel.Pais : rascunho.Pais;
                rascunho.Tipo = rascunho.Tipo == 0 ? ValoresNulosModel.Tipo : rascunho.Tipo;
                rascunho.Link = string.IsNullOrEmpty(rascunho.Link) ? ValoresNulosModel.Link : rascunho.Link;
                rascunho.Data = rascunho.Data == default ? ValoresNulosModel.Data : rascunho.Data;
                rascunho.Categorias = string.IsNullOrEmpty(rascunho.Categorias) ? ValoresNulosModel.Categorias : rascunho.Categorias;
                rascunho.Imagem = string.IsNullOrEmpty(rascunho.Imagem) ? ValoresNulosModel.ImagemNula : rascunho.Imagem;
                // Salvar alterações no rascunho
                _conteudoRascunhoRepositorio.Adicionar(rascunho, usuarioId);
                return RedirectToAction("Rascunhos");
            }
            else if (acao == "SalvarAprovar")
            {
                // Validação rigorosa para campos obrigatórios
                var erros = new List<string>();

                if (string.IsNullOrEmpty(rascunho.Nome))
                    erros.Add("Nome/Título");
                if (string.IsNullOrEmpty(rascunho.Descricao))
                    erros.Add("Descrição");
                if (rascunho.Pais == 0)
                    erros.Add("País");
                if (rascunho.Tipo == 0)
                    erros.Add("Tipo");
                if (string.IsNullOrEmpty(rascunho.Link))
                    erros.Add("Link");
                if (rascunho.Data == default)
                    erros.Add("Data");

                // Se houver erros, retorna mensagem detalhada
                if (erros.Any())
                {
                    return BadRequest($"Os seguintes campos são obrigatórios e não podem estar vazios: {string.Join(", ", erros)}");
                }

                // Enviar conteúdo para aprovação (tabela ConteudoAprovar)
                var conteudoAprovar = new ConteudoAprovarModel
                {
                    Nome = rascunho.Nome,
                    Descricao = rascunho.Descricao,
                    Pais = rascunho.Pais,
                    Tipo = rascunho.Tipo,
                    Link = rascunho.Link,
                    Imagem = string.IsNullOrEmpty(rascunho.Imagem) ? ValoresNulosModel.ImagemNula : rascunho.Imagem,
                    Data = rascunho.Data,
                    Categorias = string.IsNullOrEmpty(rascunho.Categorias) ? ValoresNulosModel.Categorias : rascunho.Categorias
                };

                _conteudoAprovarRepositorio.Adicionar(conteudoAprovar);
                return RedirectToAction("Rascunhos");
            }
            return BadRequest("Ação inválida.");
        }


        [HttpGet]
        public IActionResult EditarRascunhos(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            var rascunho = _conteudoRascunhoRepositorio.ListarPorId(id, usuarioId);

            if (rascunho == null)
            {
                return NotFound();
            }
            var viewModel = new RascunhoViewModel
            {
                Rascunho = rascunho
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AlterarRascunho(ConteudoRascunhoModel rascunho, IFormFile imagem, string acao)
        {
            string usuarioId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }

            var rascunhoAtual = _conteudoRascunhoRepositorio.ListarPorId(rascunho.Id, usuarioId);
            if (rascunhoAtual == null)
            {
                return NotFound("Rascunho não encontrado.");
            }

            // Lógica para atualizar a imagem
            if (imagem != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");

                // Remover imagem antiga se existir
                if (!string.IsNullOrEmpty(rascunhoAtual.Imagem))
                {
                    string imagemAntigaPath = Path.Combine(uploadsFolder, rascunhoAtual.Imagem);
                    if (System.IO.File.Exists(imagemAntigaPath))
                    {
                        System.IO.File.Delete(imagemAntigaPath);
                    }
                }
                // Salvar nova imagem
                string novoNomeImagem = Guid.NewGuid().ToString() + "_" + imagem.FileName;
                string novaImagemPath = Path.Combine(uploadsFolder, novoNomeImagem);
                using (var stream = new FileStream(novaImagemPath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                rascunhoAtual.Imagem = novoNomeImagem;
            }

            // Tratamento de categorias
            if (!string.IsNullOrWhiteSpace(rascunho.Categorias))
            {
                var categoriasArray = rascunho.Categorias
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (categoriasArray.Length > 7)
                {
                    return BadRequest("Você pode adicionar no máximo 7 categorias.");
                }

                rascunhoAtual.Categorias = string.Join(", ", categoriasArray) + ".";
            }
            else
            {
                rascunhoAtual.Categorias = null;
            }

            // Atualizar campos do rascunho
            rascunhoAtual.Nome = rascunho.Nome;
            rascunhoAtual.Descricao = rascunho.Descricao;
            rascunhoAtual.Pais = rascunho.Pais;
            rascunhoAtual.Tipo = rascunho.Tipo;
            rascunhoAtual.Link = rascunho.Link;
            rascunhoAtual.Data = rascunho.Data;

            if (acao == "SalvarRascunho")
            {
                _conteudoRascunhoRepositorio.Atualizar(rascunhoAtual, usuarioId);
                return RedirectToAction("Rascunhos");
            }
            else if (acao == "SalvarAprovar")
            {
                // Validação mais detalhada
                var camposObrigatorios = new List<string>();

                if (string.IsNullOrWhiteSpace(rascunhoAtual.Nome))
                    camposObrigatorios.Add("Nome");
                if (string.IsNullOrWhiteSpace(rascunhoAtual.Descricao))
                    camposObrigatorios.Add("Descrição");
                if (rascunhoAtual.Pais == 0)
                    camposObrigatorios.Add("País");
                if (rascunhoAtual.Tipo == 0)
                    camposObrigatorios.Add("Tipo");
                if (string.IsNullOrWhiteSpace(rascunhoAtual.Link))
                    camposObrigatorios.Add("Link");
                if (rascunhoAtual.Data == default)
                    camposObrigatorios.Add("Data");
                if (string.IsNullOrWhiteSpace(rascunhoAtual.Categorias))
                    camposObrigatorios.Add("Categorias");

                if (camposObrigatorios.Any())
                {
                    return BadRequest($"Os seguintes campos são obrigatórios: {string.Join(", ", camposObrigatorios)}");
                }

                var conteudoAprovar = new ConteudoAprovarModel
                {
                    Nome = rascunhoAtual.Nome,
                    Descricao = rascunhoAtual.Descricao,
                    Pais = rascunhoAtual.Pais,
                    Tipo = rascunhoAtual.Tipo,
                    Link = rascunhoAtual.Link,
                    Imagem = rascunhoAtual.Imagem,
                    Data = rascunhoAtual.Data,
                    Categorias = rascunhoAtual.Categorias
                };

                try
                {
                    _conteudoAprovarRepositorio.Adicionar(conteudoAprovar);
                    _conteudoRascunhoRepositorio.Apagar(rascunhoAtual.Id, usuarioId);
                    return RedirectToAction("Rascunhos");
                }
                catch (Exception ex)
                {
                    // Log do erro
                    return BadRequest($"Erro ao salvar: {ex.Message}");
                }
            }

            return BadRequest("Ação inválida.");
        }

        [HttpGet]
        public IActionResult DeletarRascunhos(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            ConteudoRascunhoModel rascunho = _conteudoRascunhoRepositorio.ListarPorId(id, usuarioId);
            return View(rascunho);
        }

        public IActionResult ApagarRascunho(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }

            var conteudoRascunho = _conteudoRascunhoRepositorio.ListarPorId(id, usuarioId);
            if (conteudoRascunho == null)
            {
                return NotFound("Rascunho não encontrado.");
            }

            bool sucesso = _conteudoRascunhoRepositorio.Apagar(id, usuarioId);
            if (sucesso)
            {
                // Verifica se o rascunho tem uma imagem antes de tentar deletar
                if (!string.IsNullOrEmpty(conteudoRascunho.Imagem))
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");
                    string filePath = Path.Combine(uploadsFolder, conteudoRascunho.Imagem);

                    // Só tenta deletar o arquivo se ele existir
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                return RedirectToAction("Rascunhos");
            }

            return BadRequest("Erro ao apagar o rascunho.");
        }

        [HttpPost]
        public IActionResult AdicionarRascunhoAprovar(ConteudoAprovarModel conteudoAprovar, IFormFile imagem)
        {
            // Obtendo o ID do usuário atual
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }

            // Buscar o rascunho usando o ID do ConteudoAprovar e o usuário
            var conteudoRascunho = _conteudoRascunhoRepositorio.ListarPorId(conteudoAprovar.Id, usuarioId);
            if (conteudoRascunho != null)
            {
                conteudoAprovar.Nome = string.IsNullOrEmpty(conteudoAprovar.Nome) ? conteudoRascunho.Nome : conteudoAprovar.Nome;
                conteudoAprovar.Descricao = string.IsNullOrEmpty(conteudoAprovar.Descricao) ? conteudoRascunho.Descricao : conteudoAprovar.Descricao;
                conteudoAprovar.Link = string.IsNullOrEmpty(conteudoAprovar.Link) ? conteudoRascunho.Link : conteudoAprovar.Link;
                conteudoAprovar.Pais = conteudoAprovar.Pais == ValoresNulosModel.Pais ? conteudoRascunho.Pais : conteudoAprovar.Pais;
                conteudoAprovar.Tipo = conteudoAprovar.Tipo == ValoresNulosModel.Tipo ? conteudoRascunho.Tipo : conteudoAprovar.Tipo;
                conteudoAprovar.Data = conteudoAprovar.Data == ValoresNulosModel.Data ? conteudoRascunho.Data : conteudoAprovar.Data;
                conteudoAprovar.Categorias = string.IsNullOrEmpty(conteudoAprovar.Categorias) ? conteudoRascunho.Categorias : conteudoAprovar.Categorias;
                conteudoAprovar.Imagem = conteudoAprovar.Imagem == ValoresNulosModel.ImagemNula ? conteudoRascunho.Imagem : conteudoAprovar.Imagem;
            }

            // Verificar se o conteúdo tem valores nulos ou padrão
            if (conteudoAprovar.Nome == ValoresNulosModel.Nome ||
                conteudoAprovar.Descricao == ValoresNulosModel.Descricao ||
                conteudoAprovar.Pais == ValoresNulosModel.Pais ||
                conteudoAprovar.Tipo == ValoresNulosModel.Tipo ||
                conteudoAprovar.Link == ValoresNulosModel.Link ||
                conteudoAprovar.Data == ValoresNulosModel.Data ||
                conteudoAprovar.Categorias == ValoresNulosModel.Categorias ||
                conteudoAprovar.Imagem == ValoresNulosModel.ImagemNula) // Verifica se não há categorias selecionadas
            {
                return BadRequest("Alguns campos obrigatórios estão com valores inválidos. Por favor, preencha todos os campos corretamente.");
            }

            // Tratamento da imagem
            if (imagem != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");
                string filePath = Guid.NewGuid().ToString() + "_" + imagem.FileName;
                string fullPath = Path.Combine(uploadsFolder, filePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                conteudoAprovar.Imagem = filePath;
            }
            else if (conteudoRascunho != null && !string.IsNullOrEmpty(conteudoRascunho.Imagem))
            {
                // Se a imagem não foi enviada, mas existe no rascunho, reutilize a imagem do rascunho
                conteudoAprovar.Imagem = conteudoRascunho.Imagem;
            }
            else
            {
                return BadRequest("Imagem é obrigatória.");
            }

            // Adicionando o conteúdo a ser aprovado
            _conteudoAprovarRepositorio.Adicionar(new ConteudoAprovarModel
            {
                Tipo = conteudoAprovar.Tipo,
                Nome = conteudoAprovar.Nome,
                Descricao = conteudoAprovar.Descricao,
                Link = conteudoAprovar.Link,
                Imagem = conteudoAprovar.Imagem,
                Pais = conteudoAprovar.Pais,
                Data = conteudoAprovar.Data,
                Categorias = conteudoAprovar.Categorias
            });

            // Remover o rascunho após adicionar ao ConteudoAprovar
            if (conteudoRascunho != null)
            {
                _conteudoRascunhoRepositorio.Apagar(conteudoAprovar.Id, usuarioId);
            }

            return RedirectToAction("Rascunhos");
        }



        // ================================= APROVAR/REPROVAR =================================
       
        [Authorize(Roles = "Admin")]
        public IActionResult Solicitacoes()
        {
            var conteudos = _conteudoAprovarRepositorio.PegarConteudosTemporarios();
            return View(conteudos);
        }

        public IActionResult ObterDetalhesSolicitacao(int id)
        {
            var solicitacao = _dbContext.ConteudoAprovar.FirstOrDefault(s => s.Id == id);

            if (solicitacao == null)
            {
                return NotFound();
            }

            return PartialView("_SolicitacaoDetalhes", solicitacao);
        }

        [HttpPost]
        public IActionResult Aprovar(int id)
        {
            // Obter o conteúdo temporário
            var conteudoTemp = _conteudoAprovarRepositorio.ListarPorId(id);

            // Criar um novo ConteudoModel e salvar no banco principal
            if (conteudoTemp != null)
            {
                var conteudo = new ConteudoModel
                {
                    Nome = conteudoTemp.Nome,
                    Descricao = conteudoTemp.Descricao,
                    Imagem = conteudoTemp.Imagem,
                    Link = conteudoTemp.Link,
                    Tipo = conteudoTemp.Tipo,
                    Pais = conteudoTemp.Pais,
                    Data = conteudoTemp.Data,
                    Categorias = conteudoTemp.Categorias
            };

                _conteudoRepositorio.Adicionar(conteudo);
                _conteudoAprovarRepositorio.Apagar(id);
            }

            return RedirectToAction("Solicitacoes");
        }

        [HttpPost]
        public IActionResult Rejeitar(int id)
        {
            var conteudoAprovar = _conteudoAprovarRepositorio.ListarPorId(id);

            if (conteudoAprovar != null)
            {
                // Caminho completo da imagem no diretório wwwroot/Imagens
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Imagens");
                string filePath = Path.Combine(uploadsFolder, conteudoAprovar.Imagem);

                // Verifica se o arquivo existe e o exclui
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Apaga o conteúdo do repositório
                _conteudoAprovarRepositorio.Apagar(id);
            }

            return RedirectToAction("Solicitacoes");
        }

        // ================================= CONTATO/FEEDBACK =================================
        
        [Authorize(Roles = "Admin")]
        public IActionResult Feedbacks()
        {
            var contatos = _contatoRepositorio.TodosContatos();
            return View(contatos);
        }

        public IActionResult ObterDetalhesdoFeedback(int id)
        {
            var feedback = _dbContext.Contatos.FirstOrDefault(c => c.Id == id);

            if (feedback == null)
            {
                return NotFound();
            }

            // Retorna uma partial view com os detalhes
            return PartialView("_FeedbackDetalhes", feedback);
        }

        public IActionResult ApagarContato(int id)
        {
            var contato = _contatoRepositorio.ListarPorId(id);
            if (contato != null)
            {
                _contatoRepositorio.ApagarContato(id);
            }
            return RedirectToAction("Feedbacks");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
