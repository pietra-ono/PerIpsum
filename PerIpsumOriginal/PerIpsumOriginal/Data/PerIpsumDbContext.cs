using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data.Maps;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Models.SubModels;

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
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<FavoritoModel> Favoritos { get; set; }
        public DbSet<AnotacaoModel> Anotacoes { get; set; }

        public DbSet<ConteudoRascunhoCategorias> ConteudoRascunhoCategorias { get; set; }
        public DbSet<ConteudoAprovarCategorias> ConteudoAprovarCategorias { get; set; }
        public DbSet<ConteudoCategorias> ConteudoCategorias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ConteudoMap());
            modelBuilder.ApplyConfiguration(new ConteudoAprovarMap());
            modelBuilder.ApplyConfiguration(new ConteudoRascunhoMap());
            modelBuilder.ApplyConfiguration(new FavoritoMap());

            modelBuilder.Entity<ConteudoRascunhoCategorias>()
                .HasKey(cr => new { cr.ConteudoRascunhoId, cr.CategoriaId });

            modelBuilder.Entity<ConteudoRascunhoCategorias>()
                .HasOne(cr => cr.ConteudoRascunho)
                .WithMany(c => c.ConteudoRascunhoCategorias)
                .HasForeignKey(cr => cr.ConteudoRascunhoId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ConteudoRascunhoCategorias>()
                .HasOne(cr => cr.Categoria)
                .WithMany(c => c.ConteudoRascunhoCategorias)
                .HasForeignKey(cr => cr.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConteudoAprovarCategorias>()
                .HasKey(ca => new { ca.ConteudoAprovarId, ca.CategoriaId });

            modelBuilder.Entity<ConteudoAprovarCategorias>()
                .HasOne(ca => ca.ConteudoAprovar)
                .WithMany(c => c.ConteudoAprovarCategorias)
                .HasForeignKey(ca => ca.ConteudoAprovarId)
                .OnDelete(DeleteBehavior.Cascade); // Delete em cascata

            modelBuilder.Entity<ConteudoAprovarCategorias>()
                .HasOne(ca => ca.Categoria)
                .WithMany(c => c.ConteudoAprovarCategorias)
                .HasForeignKey(ca => ca.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ConteudoCategorias>()
                .HasKey(c => new { c.ConteudoId, c.CategoriaId });

            modelBuilder.Entity<ConteudoCategorias>()
               .HasOne(ca => ca.Conteudo)
               .WithMany(c => c.ConteudoCategorias)
               .HasForeignKey(ca => ca.ConteudoId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConteudoCategorias>()
                .HasOne(ca => ca.Categoria)
                .WithMany(c => c.ConteudoCategorias)
                .HasForeignKey(ca => ca.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ConteudoRascunhoModel>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoritoModel>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FavoritoModel>()
                .HasOne(a => a.Conteudo)
                .WithMany()
                .HasForeignKey(a => a.ConteudoId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AnotacaoModel>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
