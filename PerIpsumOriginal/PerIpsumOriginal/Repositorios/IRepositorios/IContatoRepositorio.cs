using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface IContatoRepositorio
    {
        ContatoModel AdicionarContato(ContatoModel contato);
        ContatoModel ListarPorId(int id);
        IEnumerable<ContatoModel> TodosContatos();
        bool ApagarContato(int id);
    }
}
