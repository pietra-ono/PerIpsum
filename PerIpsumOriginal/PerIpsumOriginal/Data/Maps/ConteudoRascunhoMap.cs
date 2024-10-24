using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data.Maps
{
    public class ConteudoRascunhoMap : IEntityTypeConfiguration<ConteudoRascunhoModel>
    {
        public void Configure(EntityTypeBuilder<ConteudoRascunhoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Tipo);
            builder.Property(x => x.Nome).HasMaxLength(100);
            builder.Property(x => x.Descricao).HasMaxLength(300);
            builder.Property(x => x.Link).HasMaxLength(100);
            builder.Property(x => x.Imagem);
            builder.Property(x => x.Data).HasColumnType("date");
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.HasOne(x => x.Usuario);
        }
    }
}
