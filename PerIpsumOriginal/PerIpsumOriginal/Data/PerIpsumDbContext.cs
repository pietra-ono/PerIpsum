using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data.Maps;
using PerIpsumOriginal.Migrations;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.Data
{
    public class PerIpsumDbContext : IdentityDbContext<UsuarioModel>
    {
        public PerIpsumDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ContatoModel> Contatos { get; set; }
        public DbSet<ConteudoModel> Conteudos { get; set; }
        public DbSet<ConteudoAprovarModel> ConteudoAprovar { get; set; }
        public DbSet<ConteudoRascunhoModel> ConteudoRascunho { get; set; }
        public DbSet<FavoritoModel> Favoritos { get; set; }
        public DbSet<AnotacaoModel> Anotacoes { get; set; }
        public DbSet<CalendarioModel> Calendario { get; set; }
        public DbSet<PreferenciasModel> Preferencias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoMap());
            modelBuilder.ApplyConfiguration(new ConteudoMap());
            modelBuilder.ApplyConfiguration(new ConteudoAprovarMap());
            modelBuilder.ApplyConfiguration(new ConteudoRascunhoMap());

            modelBuilder.Entity<ConteudoRascunhoModel>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnotacaoModel>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
           

            modelBuilder.Entity<PreferenciasModel>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
