using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data.Maps
{
    public class AnotacaoMap : IEntityTypeConfiguration<AnotacaoModel>
    {
        public void Configure(EntityTypeBuilder<AnotacaoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Cor).IsRequired().HasMaxLength(7);
            builder.Property(x => x.UsuarioId).IsRequired();

            builder.HasOne(x => x.Usuario);
        }
    }
}
