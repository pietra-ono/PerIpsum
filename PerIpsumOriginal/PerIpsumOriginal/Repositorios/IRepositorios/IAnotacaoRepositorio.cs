using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IAnotacaoRepositorio
    {
        IEnumerable<AnotacaoModel> ObterAnotacoesPorUsuario(string usuarioId);
        AnotacaoModel Adicionar(AnotacaoModel anotacao, string usuarioId);
        AnotacaoModel Atualizar(AnotacaoModel anotacao, string usuarioId);
        AnotacaoModel ListarPorId(int id, string usuarioId);
        bool Apagar(int id, string usuarioId);
    }
}
