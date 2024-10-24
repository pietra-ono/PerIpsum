using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Models.SubModels;
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
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly PerIpsumDbContext _dbContext;

        public AdminController(IContatoRepositorio contatoRepositorio,
            IConteudoRascunhoRepositorio conteudoRascunhoRepositorio,
            IConteudoAprovarRepositorio conteudoAprovarRepositorio,
            IConteudoRepositorio conteudoRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IWebHostEnvironment environment,
            UserManager<UsuarioModel> userManager,
            PerIpsumDbContext dbContext
            )
        {
            _contatoRepositorio = contatoRepositorio;
            _conteudoRascunhoRepositorio = conteudoRascunhoRepositorio;
            _conteudoAprovarRepositorio = conteudoAprovarRepositorio;
            _conteudoRepositorio = conteudoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
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
                return BadRequest("O usuário não está logado.");
            }

            var rascunhos = _conteudoRascunhoRepositorio.ObterRascunhosPorUsuario(usuarioId);
            var categorias = _dbContext.Categorias.ToList();
            var categoriasSelectList = categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();

            var viewModel = new RascunhoViewModel
            {
                Rascunhos = rascunhos,
                Categorias = categoriasSelectList
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AdicionarRascunho(ConteudoRascunhoModel rascunho, IFormFile imagem, int[] categorias)
        {
            string usuarioId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }

            rascunho.UsuarioId = usuarioId;
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
            if (categorias != null)
            {
                foreach (var categoriaId in categorias)
                {
                    if (!_dbContext.ConteudoRascunhoCategorias.Any(crc => crc.ConteudoRascunhoId == rascunho.Id && crc.CategoriaId == categoriaId))
                    {
                        var categoria = new ConteudoRascunhoCategorias
                        {
                            ConteudoRascunhoId = rascunho.Id,
                            CategoriaId = categoriaId
                        };
                        _dbContext.ConteudoRascunhoCategorias.Add(categoria);
                    }
                }
            }
            _conteudoRascunhoRepositorio.Adicionar(rascunho, usuarioId, categorias);
            return RedirectToAction("Rascunhos");
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

            var categorias = _dbContext.Categorias.ToList();
            var categoriasSelectList = categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();

            var viewModel = new RascunhoViewModel
            {
                Rascunho = rascunho,  // Passando o rascunho diretamente
                Categorias = categoriasSelectList
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AlterarRascunho(ConteudoRascunhoModel rascunho, IFormFile imagemArquivo, int[] categorias, string acao)
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

            // Manipulação da imagem
            if (imagemArquivo != null)
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
                string novoNomeImagem = Guid.NewGuid().ToString() + "_" + imagemArquivo.FileName;
                string novaImagemPath = Path.Combine(uploadsFolder, novoNomeImagem);
                using (var stream = new FileStream(novaImagemPath, FileMode.Create))
                {
                    await imagemArquivo.CopyToAsync(stream);
                }
                rascunhoAtual.Imagem = novoNomeImagem;
            }

            // Atualizar categorias
            if (categorias != null)
            {
                var categoriasARemover = rascunhoAtual.ConteudoRascunhoCategorias
                    .Where(crc => crc.ConteudoRascunhoId == rascunhoAtual.Id && !categorias.Contains(crc.CategoriaId))
                    .ToList();

                foreach (var categoria in categoriasARemover)
                {
                    rascunhoAtual.ConteudoRascunhoCategorias.Remove(categoria);
                }

                foreach (var categoriaId in categorias)
                {
                    if (!_dbContext.ConteudoRascunhoCategorias.Any(crc => crc.ConteudoRascunhoId == rascunhoAtual.Id && crc.CategoriaId == categoriaId))
                    {
                        var novaRelacao = new ConteudoRascunhoCategorias
                        {
                            ConteudoRascunhoId = rascunhoAtual.Id,
                            CategoriaId = categoriaId
                        };
                        _dbContext.ConteudoRascunhoCategorias.Add(novaRelacao);
                    }
                }
            }

            // Atualizar campos do rascunho
            rascunhoAtual.Nome = rascunho.Nome;
            rascunhoAtual.Descricao = rascunho.Descricao;
            rascunhoAtual.Pais = rascunho.Pais;
            rascunhoAtual.Tipo = rascunho.Tipo;
            rascunhoAtual.Link = rascunho.Link;
            rascunhoAtual.Data = rascunho.Data;

            // Verifica a ação selecionada pelo usuário
            if (acao == "SalvarRascunho")
            {
                // Salvar alterações no rascunho
                _conteudoRascunhoRepositorio.Atualizar(rascunhoAtual, usuarioId, categorias);
                return RedirectToAction("Rascunhos");
            }
            else if (acao == "EnviarAprovar")
            {
                // Validação para verificar valores "nulos" ou inválidos
                if (rascunhoAtual.Nome == ValoresNulosModel.Nome ||
                    rascunhoAtual.Descricao == ValoresNulosModel.Descricao ||
                    rascunhoAtual.Pais == ValoresNulosModel.Pais ||
                    rascunhoAtual.Tipo == ValoresNulosModel.Tipo ||
                    rascunhoAtual.Link == ValoresNulosModel.Link ||
                    rascunhoAtual.Data == ValoresNulosModel.Data)
                {
                    return BadRequest("Alguns campos obrigatórios estão com valores inválidos. Por favor, preencha todos os campos corretamente.");
                }

                // Enviar conteúdo para aprovação (tabela ConteudoAprovar)
                var conteudoAprovar = new ConteudoAprovarModel
                {
                    Nome = rascunhoAtual.Nome,
                    Descricao = rascunhoAtual.Descricao,
                    Pais = rascunhoAtual.Pais,
                    Tipo = rascunhoAtual.Tipo,
                    Link = rascunhoAtual.Link,
                    Imagem = rascunhoAtual.Imagem,
                    Data = rascunhoAtual.Data
                };

                _conteudoAprovarRepositorio.Adicionar(conteudoAprovar);
                await _dbContext.SaveChangesAsync();

                // Copiar as categorias associadas para ConteudoAprovarCategorias
                var categoriasRascunho = await _dbContext.ConteudoRascunhoCategorias
                    .Where(crc => crc.ConteudoRascunhoId == rascunhoAtual.Id)
                    .ToListAsync();

                foreach (var categoriaRascunho in categoriasRascunho)
                {
                    var conteudoAprovarCategoria = new ConteudoAprovarCategorias
                    {
                        ConteudoAprovarId = conteudoAprovar.Id,
                        CategoriaId = categoriaRascunho.CategoriaId
                    };
                    _dbContext.ConteudoAprovarCategorias.Add(conteudoAprovarCategoria);
                }

                await _dbContext.SaveChangesAsync();

                // Apagar o rascunho depois de enviar para aprovação
                _conteudoRascunhoRepositorio.Apagar(rascunhoAtual.Id, usuarioId);

                return RedirectToAction("Rascunhos");
            }

            // Caso a ação seja inválida, retornar um erro
            return BadRequest("Ação inválida.");
        }





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
                // Atualizar os valores do ConteudoAprovar com base no rascunho, caso os campos sejam nulos ou não preenchidos
                conteudoAprovar.Nome = string.IsNullOrEmpty(conteudoAprovar.Nome) ? conteudoRascunho.Nome : conteudoAprovar.Nome;
                conteudoAprovar.Descricao = string.IsNullOrEmpty(conteudoAprovar.Descricao) ? conteudoRascunho.Descricao : conteudoAprovar.Descricao;
                conteudoAprovar.Link = string.IsNullOrEmpty(conteudoAprovar.Link) ? conteudoRascunho.Link : conteudoAprovar.Link;
                conteudoAprovar.Pais = conteudoAprovar.Pais == ValoresNulosModel.Pais ? conteudoRascunho.Pais : conteudoAprovar.Pais;
                conteudoAprovar.Tipo = conteudoAprovar.Tipo == ValoresNulosModel.Tipo ? conteudoRascunho.Tipo : conteudoAprovar.Tipo;
                conteudoAprovar.Data = conteudoAprovar.Data == ValoresNulosModel.Data ? conteudoRascunho.Data : conteudoAprovar.Data;
                conteudoAprovar.ConteudoAprovarCategorias = conteudoAprovar.ConteudoAprovarCategorias ?? conteudoRascunho.ConteudoRascunhoCategorias.Select(c => new ConteudoAprovarCategorias { CategoriaId = c.CategoriaId }).ToList();

            }

            // Verificar se o conteúdo tem valores nulos ou padrão
            if (conteudoAprovar.Nome == ValoresNulosModel.Nome ||
                conteudoAprovar.Descricao == ValoresNulosModel.Descricao ||
                conteudoAprovar.Pais == ValoresNulosModel.Pais ||
                conteudoAprovar.Tipo == ValoresNulosModel.Tipo ||
                conteudoAprovar.Link == ValoresNulosModel.Link ||
                conteudoAprovar.Data == ValoresNulosModel.Data)
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
                ConteudoAprovarCategorias = conteudoAprovar.ConteudoAprovarCategorias// Certificar que as categorias estão sendo adicionadas
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
                    ConteudoCategorias = conteudoTemp.ConteudoAprovarCategorias
                .Select(ca => new ConteudoCategorias { CategoriaId = ca.CategoriaId })
                .ToList()
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
