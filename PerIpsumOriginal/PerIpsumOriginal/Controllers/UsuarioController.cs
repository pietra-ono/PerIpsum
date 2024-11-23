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
            var preferencias = _dbContext.Preferencias
                .Where(p => p.UsuarioId == userId)
                .ToList();

            var tiposSelecionados = preferencias.Select(p => p.Tipo).ToList();
            var paisesSelecionados = preferencias.Select(p => p.Pais).ToList();

            var viewModel = new RascunhoViewModel
            {
                Conteudos = _dbContext.Conteudos.ToList(),
                FavoritosIds = favoritosIds ?? new List<int>(), // Garantir que a lista não seja nula
                TiposSelecionados = tiposSelecionados,
                PaisesSelecionados = paisesSelecionados
            };

            return View(viewModel);
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
                isUserEvent = false,
                color = c.Tipo == TipoEnum.Provas ? "green" :
                    c.Tipo == TipoEnum.Oportunidades ? "yellow" :
                    c.Tipo == TipoEnum.Eventos ? "blue" : "gray"
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
                    isUserEvent = true,
                    color = "purple"
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

            return View(favoritos);
        }


        [HttpPost]
        [Route("Favoritos/ToggleFavorito")]
        public IActionResult ToggleFavorito(int conteudoId)
        {
            try
            {
                // Obtenha o usuário logado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return BadRequest("Usuário não autenticado.");
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

        // ================================= PREFERÊNCIAS =================================

        [HttpGet]
        public IActionResult Preferencias()
        {
            var tipos = Enum.GetValues(typeof(TipoEnum)).Cast<TipoEnum>().ToList();
            var paises = Enum.GetValues(typeof(PaisEnum)).Cast<PaisEnum>().ToList();

            return View(new PreferenciasViewModel { Tipos = tipos, Paises = paises });
        }

        [HttpPost]
        public async Task<IActionResult> SalvarPreferencias(PreferenciasViewModel model)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Remover preferências existentes
            var existingPreferences = await _dbContext.Preferencias
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();

            _dbContext.Preferencias.RemoveRange(existingPreferences);

            // Adicionar novas preferências
            if (model.SelectedTipos != null)
            {
                foreach (var tipo in model.SelectedTipos)
                {
                    _dbContext.Preferencias.Add(new PreferenciasModel
                    {
                        UsuarioId = usuarioId,
                        Tipo = tipo
                    });
                }
            }

            if (model.SelectedPaises != null)
            {
                foreach (var pais in model.SelectedPaises)
                {
                    _dbContext.Preferencias.Add(new PreferenciasModel
                    {
                        UsuarioId = usuarioId,
                        Pais = pais
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Usuario");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
