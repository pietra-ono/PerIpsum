using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IConteudoAprovarRepositorio
    {
        ConteudoAprovarModel Adicionar(ConteudoAprovarModel conteudoAprovar);
        bool Apagar(int id);
        IEnumerable<ConteudoAprovarModel> PegarConteudosTemporarios();
        ConteudoAprovarModel ListarPorId(int id);
    }
}
