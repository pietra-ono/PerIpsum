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
            builder.Property(x => x.Tipo).IsRequired();
            builder.Property(x => x.Pais).IsRequired();
            builder.Property(x => x.Nome).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Descricao).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Link).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Imagem).IsRequired();                              
            builder.Property(x => x.Data).HasColumnType("date").IsRequired();
            builder.Property(x => x.Categorias).IsRequired();
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.HasOne(x => x.Usuario);
        }
    }
}
