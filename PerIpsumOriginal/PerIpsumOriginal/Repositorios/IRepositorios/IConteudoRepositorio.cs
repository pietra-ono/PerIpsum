using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IConteudoRepositorio
    {
        ConteudoModel Adicionar(ConteudoModel conteudo);
        ConteudoModel ListarPorId(int id);
        IEnumerable<ConteudoModel> PegarConteudos();
        bool Apagar(int id);
        List<ConteudoModel> ObterConteudosPorIds(List<int> conteudoIds);

    }
}
