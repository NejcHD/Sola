using Backend.Entitete;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class BazaContext : DbContext
    {
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Soba> Sobe { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=baza.db");
        }
    }
}
