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
using System.Linq;
using System.Security.Claims;

namespace PerIpsumOriginal.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly IConteudoRepositorio _conteudoRepositorio;
        private readonly IAnotacaoRepositorio _anotacaoRepositorio;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly PerIpsumDbContext _dbContext;

        public UsuarioController(IContatoRepositorio contatoRepositorio,
            IConteudoRepositorio conteudoRepositorio,
            IAnotacaoRepositorio anotacaoRepositorio,
            IWebHostEnvironment environment,
            UserManager<UsuarioModel> userManager,
            PerIpsumDbContext dbContext
            )
        {
            _contatoRepositorio = contatoRepositorio;
            _conteudoRepositorio = conteudoRepositorio;
            _anotacaoRepositorio = anotacaoRepositorio;
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
            var userId = _userManager.GetUserId(User);

            var favoritosIds = _dbContext.Favoritos
                .Where(f => f.UsuarioId == userId)
                .Select(f => f.ConteudoId)
                .ToList();

            var viewModel = new RascunhoViewModel
            {
                Conteudos = _dbContext.Conteudos.ToList(),
                FavoritosIds = favoritosIds ?? new List<int>(),
                TiposSelecionados = new List<TipoEnum>(), // Adicione a lógica para carregar os tipos selecionados
                PaisesSelecionados = new List<PaisEnum>() // Adicione a lógica para carregar os países selecionados
            };

            return View(viewModel);
        }


        [HttpPost]
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

            return Json(new { success = true }); // Retorna um JSON indicando sucesso
        }

        // ================================= CALENDÁRIO =================================

        [HttpGet]
        public IActionResult Calendario()
        {
            return View(); // Renderiza a View HTML com o calendário
        }

        public async Task<JsonResult> GetEventos()
        {
            var userId = _userManager.GetUserId(User);

            var eventosConteudo = _dbContext.Conteudos.Select(c => new
            {
                id = c.Id,
                title = c.Nome,
                start = c.Data.ToString("yyyy-MM-dd"),
                description = c.Descricao,
                link = c.Link,
                tipo = c.Tipo.ToString(),
                pais = c.Pais.ToString(),
                bandeira = $"/img/bandeiras/{c.Pais}.svg",
                isUserEvent = false,
                color = c.Tipo == TipoEnum.Bolsas ? "#C50003" :
                    c.Tipo == TipoEnum.Intercambios ? "#E2CB26" :
                    c.Tipo == TipoEnum.Programas ? "#642C8F" :
                    c.Tipo == TipoEnum.Estagios ? "#009846" :
                    c.Tipo == TipoEnum.Cursos ? "#002279" :
                    c.Tipo == TipoEnum.Eventos ? "#931486" : "#71717180"
            }).ToList();

            var eventosUsuario = _dbContext.Calendario
                .Where(e => e.UsuarioId == userId)
                .Select(e => new
                {
                    id = e.Id,
                    title = e.Titulo,
                    start = e.Data.ToString("yyyy-MM-dd"),
                    description = e.Descricao,
                    link = (string)null,
                    tipo = (string)null,
                    pais = (string)null,
                    bandeira = (string)null,
                    isUserEvent = true,
                    color = "gray"
                }).ToList();

            var todosEventos = eventosConteudo.Concat(eventosUsuario);
            return Json(todosEventos);
        }


        [HttpPost]
        public async Task<JsonResult> CriarEvento([FromBody] CalendarioModel evento)
        {
            evento.UsuarioId = _userManager.GetUserId(User);
            _dbContext.Calendario.Add(evento);
            await _dbContext.SaveChangesAsync();
            return Json(new { success = true, id = evento.Id });
        }

        [HttpPut]
        public async Task<JsonResult> AtualizarEvento([FromBody] CalendarioModel evento)
        {
            var userId = _userManager.GetUserId(User);
            var eventoExistente = await _dbContext.Calendario
                .FirstOrDefaultAsync(e => e.Id == evento.Id && e.UsuarioId == userId);

            if (eventoExistente != null)
            {
                eventoExistente.Titulo = evento.Titulo;
                eventoExistente.Descricao = evento.Descricao;
                eventoExistente.Data = evento.Data;
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpDelete]
        public async Task<JsonResult> DeletarEvento(int id)
        {
            var userId = _userManager.GetUserId(User);
            var evento = await _dbContext.Calendario
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == userId);

            if (evento != null)
            {
                _dbContext.Calendario.Remove(evento);
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
            var userId = _userManager.GetUserId(User);

            var favoritos = await _dbContext.Favoritos
                .Where(f => f.UsuarioId == userId)
                .Include(f => f.Conteudo)
                .Select(f => f.Conteudo)
                .ToListAsync();

            var viewModel = new RascunhoViewModel
            {
                Conteudos = favoritos,
                FavoritosIds = favoritos.Select(f => f.Id).ToList(),
                TiposSelecionados = new List<TipoEnum>(),
                PaisesSelecionados = new List<PaisEnum>()
            };

            return View(viewModel);
        }


        [HttpPost]
        [Route("Favoritos/ToggleFavorito")]
        public IActionResult ToggleFavorito(int conteudoId)
        {
            try
            {
                // Verifica se o usuário está autenticado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("/Identity/Account/Register"); // Retorna o URL da página de cadastro
                }

                var conteudo = _dbContext.Conteudos.FirstOrDefault(c => c.Id == conteudoId);
                if (conteudo == null)
                {
                    return NotFound("Conteúdo não encontrado.");
                }

                var favorito = _dbContext.Favoritos
                    .FirstOrDefault(f => f.UsuarioId == userId && f.ConteudoId == conteudoId);

                if (favorito == null)
                {
                    // Adiciona aos favoritos
                    _dbContext.Favoritos.Add(new FavoritoModel
                    {
                        UsuarioId = userId,
                        ConteudoId = conteudoId
                    });
                }
                else
                {
                    // Remove dos favoritos
                    _dbContext.Favoritos.Remove(favorito);
                }

                _dbContext.SaveChanges();

                return Ok(); // Retorna 200 OK
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao processar a solicitação.");
            }
        }




        // ================================= ANOTAÇÕES =================================

        [HttpGet]
        public IActionResult Anotacoes()
        {
            var usuarioId = _userManager.GetUserId(User);
            var anotacoes = _anotacaoRepositorio.ObterAnotacoesPorUsuario(usuarioId);
            var viewModel = new RascunhoViewModel
            {
                Anotacoes = anotacoes
            };

            return View(viewModel);
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

        [HttpPost]
        public IActionResult AlterarAnotacao([FromBody] AnotacaoModel anotacao)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            var anotacaoAtual = _anotacaoRepositorio.ListarPorId(anotacao.Id, usuarioId);
            if (anotacaoAtual == null)
            {
                return Json(new { success = false, message = "Anotação não encontrada" });
            }

            // Atualizar os campos necessários
            anotacaoAtual.Titulo = anotacao.Titulo;
            anotacaoAtual.Descricao = anotacao.Descricao;
            anotacaoAtual.Cor = anotacao.Cor;

            _anotacaoRepositorio.Atualizar(anotacaoAtual, usuarioId);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult ApagarAnotacao(int id)
        {
            string usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Json(new { success = false, message = "Usuário não autenticado" });
            }

            var anotacoes = _anotacaoRepositorio.ListarPorId(id, usuarioId);
            if (anotacoes != null)
            {
                _anotacaoRepositorio.Apagar(id, usuarioId);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Anotação não encontrada" });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
