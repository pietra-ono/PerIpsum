using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class ConteudoAprovarRepositorio : IConteudoAprovarRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public ConteudoAprovarRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ConteudoAprovarModel Adicionar(ConteudoAprovarModel conteudoAprovar)
        {
            _dbContext.ConteudoAprovar.Add(conteudoAprovar);
            _dbContext.SaveChanges();
            return conteudoAprovar;
        }

        public bool Apagar(int id)
        {
            ConteudoAprovarModel conteudoDB = ListarPorId(id);

            if (conteudoDB == null) throw new Exception("Erro na exclusão");

            _dbContext.ConteudoAprovar.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public ConteudoAprovarModel ListarPorId(int id)
        {
            return _dbContext.ConteudoAprovar.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ConteudoAprovarModel> PegarConteudosTemporarios()
        {
            return _dbContext.ConteudoAprovar.ToList();
        }
    }
}
