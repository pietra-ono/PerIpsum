using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;

namespace PerIpsumOriginal.Repositorios
{
    public class ConteudoRascunhoRepositorio : IConteudoRascunhoRepositorio
    {
        private readonly PerIpsumDbContext _dbContext;

        public ConteudoRascunhoRepositorio(PerIpsumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ConteudoRascunhoModel Adicionar(ConteudoRascunhoModel conteudoRascunho, string usuarioId)
        {
            conteudoRascunho.UsuarioId = usuarioId;
            _dbContext.ConteudoRascunho.Add(conteudoRascunho);
            _dbContext.SaveChanges();
            return conteudoRascunho;
        }

        public bool Apagar(int id, string usuarioId)
        {
            ConteudoRascunhoModel conteudoDB = ListarPorId(id, usuarioId);
            _dbContext.ConteudoRascunho.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public ConteudoRascunhoModel Atualizar(ConteudoRascunhoModel conteudoRascunho, string usuarioId)
        {
            ConteudoRascunhoModel conteudoDB = ListarPorId(conteudoRascunho.Id, conteudoRascunho.UsuarioId);
            conteudoDB.Nome = conteudoRascunho.Nome;
            conteudoDB.Descricao = conteudoRascunho.Descricao;
            conteudoDB.Tipo = conteudoRascunho.Tipo;
            conteudoDB.Pais = conteudoRascunho.Pais;
            conteudoDB.Data = conteudoRascunho.Data;
            conteudoDB.Imagem = conteudoRascunho.Imagem;
            conteudoDB.Link = conteudoRascunho.Link;
            conteudoDB.Categorias = conteudoRascunho.Categorias;

            _dbContext.ConteudoRascunho.Update(conteudoDB);
            _dbContext.SaveChanges();
            return conteudoDB;

        }

        public ConteudoRascunhoModel ListarPorId(int id, string usuarioId)
        {
            return _dbContext.ConteudoRascunho.FirstOrDefault(x => x.Id == id && x.UsuarioId == usuarioId);
        }

        public IEnumerable<ConteudoRascunhoModel> ObterRascunhosPorUsuario(string usuarioId)
        {
            return _dbContext.ConteudoRascunho.Where( x => x.UsuarioId == usuarioId).ToList();
        }
    }
}
