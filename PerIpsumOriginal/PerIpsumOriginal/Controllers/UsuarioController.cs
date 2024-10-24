using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Migrations;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Models.SubModels;
using PerIpsumOriginal.Repositorios;
using PerIpsumOriginal.Repositorios.IRepositorios;
using PerIpsumOriginal.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PerIpsumOriginal.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly IConteudoRepositorio _conteudoRepositorio;
        private readonly IAnotacaoRepositorio _anotacaoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IFavoritoRepositorio _favoritoRepositorio;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly PerIpsumDbContext _dbContext;

        public UsuarioController(IContatoRepositorio contatoRepositorio,
            IConteudoRepositorio conteudoRepositorio,
            IAnotacaoRepositorio anotacaoRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IFavoritoRepositorio favoritoRepositorio,
            IWebHostEnvironment environment,
            UserManager<UsuarioModel> userManager,
            PerIpsumDbContext dbContext
            )
        {
            _contatoRepositorio = contatoRepositorio;
            _conteudoRepositorio = conteudoRepositorio;
            _anotacaoRepositorio = anotacaoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _favoritoRepositorio = favoritoRepositorio;
            _environment = environment;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        // ================================= FEED/CONTEÚDO =================================
        [HttpGet]
        public IActionResult Feed()
        {
            var conteudos = _conteudoRepositorio.PegarConteudos();
            var categorias = _dbContext.Categorias.ToList();
            var categoriasSelectList = categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();

            var viewModel = new RascunhoViewModel
            {
                Conteudos = conteudos,
                Categorias = categoriasSelectList
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AdicionarConteudo(ConteudoModel conteudo, IFormFile imagem, int[] categorias)
        {
            if (imagem != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");
                string filePath = Guid.NewGuid().ToString() + "_" + imagem.FileName;
                string fullPath = Path.Combine(uploadsFolder, filePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                conteudo.Imagem = filePath;
            }

            _conteudoRepositorio.Adicionar(conteudo);

            return RedirectToAction("Feed");

        }

        public IActionResult DeletarFeed(int id)
        {
            ConteudoModel conteudo = _conteudoRepositorio.ListarPorId(id);
            return View(conteudo);
        }

        public IActionResult ApagarConteudo(int id)
        {
            var conteudo = _conteudoRepositorio.ListarPorId(id);

            if (conteudo != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "storage");
                string filePath = Path.Combine(uploadsFolder, conteudo.Imagem);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _conteudoRepositorio.Apagar(id);
            }

            return RedirectToAction("Feed");
        }


        public IActionResult Sobre()
        {
            return View();
        }
        // ================================= CALENDÁRIO =================================

        [HttpGet]
        public IActionResult Calendario()
        {
            return View();
        }


        // ================================= CONTATO =================================
        
        [HttpGet]
        public IActionResult Contato()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdicionarContato(ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                _contatoRepositorio.AdicionarContato(contato);
                return RedirectToAction("Contato");
            }
            return View(contato);
        }
        // ================================= FAVORITOS =================================

        [HttpGet]
        public async Task<IActionResult> Favoritos()
        {
            var usuarioId = _userManager.GetUserId(User);
            var favoritos = _favoritoRepositorio.ObterFavoritosPorUsuario(usuarioId);

            // Busca os conteúdos favoritados pelo usuário
            var conteudosFavoritos = _conteudoRepositorio.ObterConteudosPorIds(favoritos.Select(f => f.ConteudoId).ToList());

            var viewModel = new RascunhoViewModel
            {
                Conteudos = conteudosFavoritos,
                Favoritos = favoritos
            };

            return View(viewModel);// Retorna a view com os conteúdos favoritados
        }

        [HttpPost]
        public IActionResult Favoritar(FavoritoModel favorito)
        {
            try
            {
                var usuarioId = _userManager.GetUserId(User);
                var conteudoId = favorito.ConteudoId;

                var favoritoExistente = _favoritoRepositorio.ObterFavoritoPorUsuarioEConteudo(usuarioId, conteudoId);

                if (favoritoExistente != null)
                {
                    _favoritoRepositorio.RemoverFavorito(usuarioId, favoritoExistente);
                    return Json(new { success = true, isFavorited = false });
                }
                else
                {
                    _favoritoRepositorio.AdicionarFavorito(usuarioId, favorito);
                    return Json(new { success = true, isFavorited = true });
                }
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocorreu um erro inesperado" });
            }
        }

        // ================================= ANOTAÇÕES =================================

        [HttpGet]
        public IActionResult Anotacoes()
        {
            var usuarioId = _userManager.GetUserId(User);
            var anotacoes = _anotacaoRepositorio.ObterAnotacoesPorUsuario(usuarioId);
            return View(anotacoes);
        }

        [HttpPost]
        public IActionResult AdicionarAnotacao(AnotacaoModel anotacao)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }
            anotacao.UsuarioId = usuarioId;
            if (string.IsNullOrEmpty(anotacao.Cor))
            {
                anotacao.Cor = "#000000";
            }
            _anotacaoRepositorio.Adicionar(anotacao, usuarioId);
            return RedirectToAction("Anotacoes");
        }

        public IActionResult EditarAnotacoes(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            var anotacoes = _anotacaoRepositorio.ListarPorId(id, usuarioId);
            if (anotacoes == null)
            {
                return NotFound();
            }
            return View(anotacoes);
        }

        [HttpPost]
        public IActionResult AlterarAnotacao(AnotacaoModel anotacao)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }

            var anotacaoAtual = _anotacaoRepositorio.ListarPorId(anotacao.Id, usuarioId);
            if (anotacaoAtual == null)
            {
                return NotFound("Anotação não encontrada.");
            }

            // Atualizar os campos necessários
            anotacaoAtual.Titulo = anotacao.Titulo;
            anotacaoAtual.Descricao = anotacao.Descricao;
            anotacaoAtual.Cor = anotacao.Cor;

            _anotacaoRepositorio.Atualizar(anotacaoAtual, usuarioId);
            return RedirectToAction("Anotacoes");
        }


        public IActionResult DeletarAnotacoes(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            AnotacaoModel anotacao = _anotacaoRepositorio.ListarPorId(id, usuarioId);
            return View(anotacao);
        }

        public IActionResult ApagarAnotacao(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("O usuário não está logado.");
            }
            var anotacoes = _anotacaoRepositorio.ListarPorId(id, usuarioId);
            if (anotacoes != null)
            {
                _anotacaoRepositorio.Apagar(id, usuarioId);
            }
            return RedirectToAction("Anotacoes");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
