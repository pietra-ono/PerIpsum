using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class ConteudoRepositorio : IConteudoRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;
        public ConteudoRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public ConteudoModel Adicionar(ConteudoModel conteudo)
        {
            _dbContext.Conteudos.Add(conteudo);
            _dbContext.SaveChanges();
            return conteudo;
        }

        public bool Apagar(int id)
        {
            ConteudoModel conteudoDB = ListarPorId(id);

            if (conteudoDB == null) throw new Exception("Erro na exclusão");

            _dbContext.Conteudos.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public ConteudoModel ListarPorId(int id)
        {
            return _dbContext.Conteudos.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ConteudoModel> PegarConteudos()
        {
            return _dbContext.Conteudos.ToList();
        }

        public List<ConteudoModel> ObterConteudosPorIds(List<int> conteudoIds)
        {
            return _dbContext.Conteudos
                .Where(x => conteudoIds.Contains(x.Id))
                .ToList();
        }
    }
}
