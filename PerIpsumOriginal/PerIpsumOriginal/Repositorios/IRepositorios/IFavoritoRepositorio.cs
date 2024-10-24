using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IFavoritoRepositorio
    {
        IEnumerable<FavoritoModel> ObterFavoritosPorUsuario(string usuarioId);
        void AdicionarFavorito(string usuarioId, FavoritoModel favorito);
        void RemoverFavorito(string usuarioId, FavoritoModel favorito);
        FavoritoModel ObterFavoritoPorUsuarioEConteudo(string usuarioId, int conteudoId);
    }
}
