using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Drzava> Drzava { get; set; }   // obstaja tabela z tem podatki
        public DbSet<TipTekmovanja> Tip_Tekmovanja { get; set; }
        public DbSet<Tekma> Tekma { get; set; }
        public DbSet<Tekmovalci> Tekmovalci { get; set; }
        public DbSet<Rezultati> Rezultati { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ker so imena tabel v MySQL včasih specifična, jih tukaj povežemo
            modelBuilder.Entity<TipTekmovanja>().ToTable("Tip_Tekmovanja");
            modelBuilder.Entity<Drzava>().ToTable("Drzava");
            modelBuilder.Entity<Tekma>().ToTable("Tekma");
            modelBuilder.Entity<Tekmovalci>().ToTable("Tekmovalci");
            modelBuilder.Entity<Rezultati>().ToTable("Rezultati");
        }
    }
}