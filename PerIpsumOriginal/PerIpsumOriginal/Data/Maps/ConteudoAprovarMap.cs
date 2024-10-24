using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data.Maps
{
    public class ConteudoAprovarMap : IEntityTypeConfiguration<ConteudoAprovarModel>
    {
        public void Configure(EntityTypeBuilder<ConteudoAprovarModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Tipo).IsRequired();
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Link).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Imagem).IsRequired();
            builder.Property(x => x.Pais).IsRequired();
            builder.Property(x => x.Data).IsRequired().HasColumnType("date");


        }
    }
}
