using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Models.SubModels;
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

        public ConteudoRascunhoModel Adicionar(ConteudoRascunhoModel conteudoRascunho, string usuarioId, int[] categoriasIds)
        {
            // Atribuir o ID do usuário ao conteúdo rascunho
            conteudoRascunho.UsuarioId = usuarioId;

            // Definir valores padrão caso não tenham sido atribuídos
            conteudoRascunho.Nome ??= ValoresNulosModel.Nome;
            conteudoRascunho.Descricao ??= ValoresNulosModel.Descricao;
            conteudoRascunho.Pais = conteudoRascunho.Pais == 0 ? ValoresNulosModel.Pais : conteudoRascunho.Pais;
            conteudoRascunho.Tipo = conteudoRascunho.Tipo == 0 ? ValoresNulosModel.Tipo : conteudoRascunho.Tipo;
            conteudoRascunho.Link ??= ValoresNulosModel.Link;
            conteudoRascunho.Data = conteudoRascunho.Data == default ? ValoresNulosModel.Data : conteudoRascunho.Data;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // Adicionar o conteúdo ao banco de dados e salvar para garantir que o Id seja gerado
                _dbContext.ConteudoRascunho.Add(conteudoRascunho);
                _dbContext.SaveChanges(); // O Id do conteúdo rascunho será gerado aqui

                // Verificar se categorias estão sendo passadas
                if (categoriasIds != null && categoriasIds.Length > 0)
                {
                    // Criar as associações de categorias com o conteúdo rascunho
                    var conteudoRascunhoCategorias = categoriasIds.Select(categoriaId => new ConteudoRascunhoCategorias
                    {
                        ConteudoRascunhoId = conteudoRascunho.Id, // Usar o Id gerado
                        CategoriaId = categoriaId
                    }).ToList();

                    // Adicionar as associações ao contexto
                    _dbContext.ConteudoRascunhoCategorias.AddRange(conteudoRascunhoCategorias);

                    // Salvar as associações de categorias
                    _dbContext.SaveChanges(); // Salvar as categorias agora
                }

                // Commit the transaction
                transaction.Commit();

                return conteudoRascunho;
            }
            catch (Exception ex)
            {
                // Roll back the transaction if an error occurs
                transaction.Rollback();

                // Handle the exception
                throw;
            }
        }






        public bool Apagar(int id, string usuarioId)
        {
            ConteudoRascunhoModel conteudoDB = ListarPorId(id, usuarioId);

            _dbContext.ConteudoRascunho.Remove(conteudoDB);
            _dbContext.SaveChanges();
            return true;
        }

        public ConteudoRascunhoModel Atualizar(ConteudoRascunhoModel conteudoRascunho, string usuarioId, int[] categoriasIds)
        {
            ConteudoRascunhoModel conteudoDB = ListarPorId(conteudoRascunho.Id, conteudoRascunho.UsuarioId);

            conteudoDB.Nome = conteudoRascunho.Nome;
            conteudoDB.Descricao = conteudoRascunho.Descricao;
            conteudoDB.Pais = conteudoRascunho.Pais;
            conteudoDB.Imagem = conteudoRascunho.Imagem;
            conteudoDB.Tipo = conteudoRascunho.Tipo;
            conteudoDB.Link = conteudoRascunho.Link;
            conteudoDB.Data = conteudoRascunho.Data;
            
            if (categoriasIds != null)
            {
                // Remover as associações de categorias antigas
                var categoriasExistentes = _dbContext.ConteudoRascunhoCategorias
                    .Where(crc => crc.ConteudoRascunhoId == conteudoRascunho.Id)
                    .ToList();

                _dbContext.ConteudoRascunhoCategorias.RemoveRange(categoriasExistentes);

                // Adicionar as novas associações de categorias
                foreach (var categoriaId in categoriasIds)
                {
                    var novaAssociacao = new ConteudoRascunhoCategorias
                    {
                        ConteudoRascunhoId = conteudoRascunho.Id,
                        CategoriaId = categoriaId
                    };
                    _dbContext.ConteudoRascunhoCategorias.Add(novaAssociacao);
                }
            }

            _dbContext.ConteudoRascunho.Update(conteudoDB);
            _dbContext.SaveChanges();
            return conteudoDB;
        }

        public ConteudoRascunhoModel ListarPorId(int id, string usuarioId)
        {
            return _dbContext.ConteudoRascunho
                .Include(c => c.ConteudoRascunhoCategorias)
                .FirstOrDefault(x => x.Id == id && x.UsuarioId == usuarioId);
        }

        public IEnumerable<ConteudoRascunhoModel> ObterRascunhosPorUsuario(string usuarioId)
        {
            return _dbContext.ConteudoRascunho.Where(r => r.UsuarioId == usuarioId).ToList();
        }
    }
}
