using Microsoft.EntityFrameworkCore;

namespace AIS_16_10_3
{
    public class BazaContext : DbContext
    {
        public DbSet<Artikel> artikli { get; set; }
        //public DbSet<Kosarica> kosarice { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=baza.db");
        }

    }
}
