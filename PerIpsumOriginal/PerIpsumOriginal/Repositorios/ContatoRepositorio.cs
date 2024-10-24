using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class ContatoRepositorio : IContatoRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public ContatoRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ContatoModel AdicionarContato(ContatoModel contato)
        {
            _dbContext.Add(contato);
            _dbContext.SaveChanges();
            return contato;
        }

        public bool ApagarContato(int id)
        {
            ContatoModel conteudoDB = ListarPorId(id);

            if (conteudoDB == null) throw new Exception("Erro na exclusão");

            _dbContext.Contatos.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public ContatoModel ListarPorId(int id)
        {
            return _dbContext.Contatos.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ContatoModel> TodosContatos()
        {
            return _dbContext.Contatos.ToList();
        }
    }
}
