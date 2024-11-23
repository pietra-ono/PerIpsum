using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IConteudoRascunhoRepositorio
    {
        IEnumerable<ConteudoRascunhoModel> ObterRascunhosPorUsuario(string usuarioId);
        ConteudoRascunhoModel Adicionar(ConteudoRascunhoModel conteudoRascunho, string usuarioId);
        ConteudoRascunhoModel ListarPorId(int id, string usuarioId);
        ConteudoRascunhoModel Atualizar(ConteudoRascunhoModel conteudoRascunho, string usuarioId);
        bool Apagar(int id, string usuarioId);
    }
}
