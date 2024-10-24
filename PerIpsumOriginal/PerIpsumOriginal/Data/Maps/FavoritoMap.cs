using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data.Maps
{
    public class FavoritoMap : IEntityTypeConfiguration<FavoritoModel>
    {
        public void Configure(EntityTypeBuilder<FavoritoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.Property(x => x.ConteudoId).IsRequired();
            builder.HasOne(x => x.Usuario);
            builder.HasOne(x => x.Conteudo);
        }
    }
}
