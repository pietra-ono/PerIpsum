using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Repositorios.IRepositorios
{
    public interface ICategoriaRepositorio
    {
        CategoriaModel ListarPorId(int id);
        List<CategoriaModel> TodasCategorias();
    }
}
