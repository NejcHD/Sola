using Backend.Entitete;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class BazaContext : DbContext
    {
        public DbSet<Sporocilo> Sporocila { get; set; }
        public DbSet<Nabiralnik> Nabiralniki { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=baza.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sporocilo>()
                .HasOne(s => s.Prejemnik)
                .WithMany(n => n.PrejetaSporocila)
                .HasForeignKey(s => s.PrejemnikEmail)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sporocilo>()
                .HasOne(s => s.Posiljatelj)
                .WithMany(n => n.PoslanaSporocila)
                .HasForeignKey(s => s.PosiljateljEmail)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
