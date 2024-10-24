using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data.Maps
{
    public class ContatoMap : IEntityTypeConfiguration<ContatoModel>
    {
        public void Configure(EntityTypeBuilder<ContatoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Problema).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Data).IsRequired().HasColumnType("date");
        }
    }
}
