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
                // Salvar alterações no rascunho
                _conteudoRascunhoRepositorio.Adicionar(rascunho, usuarioId);
                return RedirectToAction("Rascunhos");
            }
            else if (acao == "SalvarAprovar")
            {
                // Validação para verificar valores "nulos" ou inválidos
                if (string.IsNullOrEmpty(rascunho.Nome) ||
                    string.IsNullOrEmpty(rascunho.Descricao) ||
                    rascunho.Pais == 0 || // Assumindo que 0 é um valor inválido para PaisEnum
                    rascunho.Tipo == 0 || // Assumindo que 0 é um valor inválido para TipoEnum
                    string.IsNullOrEmpty(rascunho.Link) ||
                    rascunho.Data == default(DateOnly))// Verifica se a data é válida
                {
                    return BadRequest("Alguns campos obrigatórios estão com valores inválidos. Por favor, preencha todos os campos corretamente.");
                }

                // Enviar conteúdo para aprovação (tabela ConteudoAprovar)
                var conteudoAprovar = new ConteudoAprovarModel
                {
                    Nome = rascunho.Nome,
                    Descricao = rascunho.Descricao,
                    Pais = rascunho.Pais,
                    Tipo = rascunho.Tipo,
                    Link = rascunho.Link,
                    Imagem = rascunho.Imagem,
                    Data = rascunho.Data,
                    Categorias = rascunho.Categorias
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
            if (!string.IsNullOrWhiteSpace(rascunho.Categorias))
            {
                var categoriasArray = rascunho.Categorias
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries); // Divide por espaço

                if (categoriasArray.Length > 7)
                {
                    return BadRequest("Você pode adicionar no máximo 7 categorias.");
                }

                // Reformatar categorias
                rascunhoAtual.Categorias = string.Join(", ", categoriasArray) + ".";
            }
            else
            {
                rascunhoAtual.Categorias = null; // Permitir que categorias sejam removidas
            }

            // Atualizar campos do rascunho
            rascunhoAtual.Nome = rascunho.Nome;
            rascunhoAtual.Descricao = rascunho.Descricao;
            rascunhoAtual.Pais = rascunho.Pais;
            rascunhoAtual.Tipo = rascunho.Tipo;
            rascunhoAtual.Link = rascunho.Link;
            rascunhoAtual.Data = rascunho.Data;
            rascunhoAtual.Categorias = rascunho.Categorias;

            if (acao == "SalvarRascunho")
            {
                _conteudoRascunhoRepositorio.Atualizar(rascunhoAtual, usuarioId);
                return RedirectToAction("Rascunhos");
            }
            else if (acao == "EnviarAprovar")
            {
                if (string.IsNullOrEmpty(rascunhoAtual.Nome) ||
                    string.IsNullOrEmpty(rascunhoAtual.Descricao) ||
                    rascunhoAtual.Pais == 0 || // Assumindo que 0 é um valor inválido para PaisEnum
                    rascunhoAtual.Tipo == 0 || // Assumindo que 0 é um valor inválido para TipoEnum
                    string.IsNullOrEmpty(rascunhoAtual.Link) ||
                    rascunhoAtual.Data == default(DateOnly) || // Verifica se a data é válida
                    string.IsNullOrEmpty(rascunhoAtual.Categorias))
                {
                    return BadRequest("Alguns campos obrigatórios estão com valores inválidos. Por favor, preencha todos os campos corretamente.");
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

                _conteudoAprovarRepositorio.Adicionar(conteudoAprovar);
                _dbContext.SaveChanges();
                _conteudoRascunhoRepositorio.Apagar(rascunhoAtual.Id, usuarioId);
                return RedirectToAction("Rascunhos");
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
            }

            // Verificar se o conteúdo tem valores nulos ou padrão
            if (conteudoAprovar.Nome == ValoresNulosModel.Nome ||
                conteudoAprovar.Descricao == ValoresNulosModel.Descricao ||
                conteudoAprovar.Pais == ValoresNulosModel.Pais ||
                conteudoAprovar.Tipo == ValoresNulosModel.Tipo ||
                conteudoAprovar.Link == ValoresNulosModel.Link ||
                conteudoAprovar.Data == ValoresNulosModel.Data ||
                conteudoAprovar.Categorias == ValoresNulosModel.Categorias) // Verifica se não há categorias selecionadas
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
        public IActionResult Solicitacoes()
        {
            var conteudos = _conteudoAprovarRepositorio.PegarConteudosTemporarios();
            return View(conteudos);
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
        public IActionResult Feedbacks()
        {
            var contatos = _contatoRepositorio.TodosContatos();
            return View(contatos);
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
