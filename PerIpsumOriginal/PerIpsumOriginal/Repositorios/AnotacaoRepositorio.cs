using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class AnotacaoRepositorio : IAnotacaoRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public AnotacaoRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AnotacaoModel Adicionar(AnotacaoModel anotacao, string usuarioId)
        {
            anotacao.UsuarioId = usuarioId;
            _dbContext.Anotacoes.Add(anotacao);
            _dbContext.SaveChanges();
            return anotacao;
        }

        public bool Apagar(int id, string usuarioId)
        {
            AnotacaoModel conteudoDB = ListarPorId(id, usuarioId);

            _dbContext.Anotacoes.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public AnotacaoModel Atualizar(AnotacaoModel anotacao, string usuarioId)
        {
            AnotacaoModel conteudoDB = ListarPorId(anotacao.Id, anotacao.UsuarioId);
            conteudoDB.Titulo = anotacao.Titulo;
            conteudoDB.Descricao = anotacao.Descricao;
            conteudoDB.Cor = anotacao.Cor;

            _dbContext.Anotacoes.Update(conteudoDB);
            _dbContext.SaveChanges();
            return conteudoDB;
        }

        public AnotacaoModel ListarPorId(int id, string usuarioId)
        {
            return _dbContext.Anotacoes.FirstOrDefault(x => x.Id == id && x.UsuarioId == usuarioId);
        }

        public IEnumerable<AnotacaoModel> ObterAnotacoesPorUsuario(string usuarioId)
        {
            return _dbContext.Anotacoes.Where(a => a.UsuarioId == usuarioId).ToList();
        }
    }
}
