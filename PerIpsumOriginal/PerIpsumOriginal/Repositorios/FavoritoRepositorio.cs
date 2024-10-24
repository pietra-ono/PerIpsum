using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class FavoritoRepositorio : IFavoritoRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public FavoritoRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<FavoritoModel> ObterFavoritosPorUsuario(string usuarioId)
        {
            return _dbContext.Favoritos
                .Include(f => f.Conteudo)
                .Where(f => f.UsuarioId == usuarioId)
                .ToList();
        }

        public void AdicionarFavorito(string usuarioId, FavoritoModel favorito)
        {
            if (string.IsNullOrEmpty(usuarioId))
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            favorito.UsuarioId = usuarioId;
            _dbContext.Favoritos.Add(favorito);
            _dbContext.SaveChanges();
        }

        public void RemoverFavorito(string usuarioId, FavoritoModel favorito)
        {
            var favoritoExistente = _dbContext.Favoritos
                .FirstOrDefault(f => f.UsuarioId == usuarioId && f.ConteudoId == favorito.ConteudoId);
            if (favoritoExistente != null)
            {
                _dbContext.Favoritos.Remove(favoritoExistente);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Favorito não encontrado");
            }
        }

        public FavoritoModel ObterFavoritoPorUsuarioEConteudo(string usuarioId, int conteudoId)
        {
            return _dbContext.Favoritos
                .FirstOrDefault(f => f.UsuarioId == usuarioId && f.ConteudoId == conteudoId);
        }
    }
}
