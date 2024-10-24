using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public CategoriaRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CategoriaModel ListarPorId(int id)
        {
            return _dbContext.Categorias.FirstOrDefault(x => x.Id == id);
        }

        public List<CategoriaModel> TodasCategorias()
        {
            return _dbContext.Categorias.ToList();
        }
    }
}
