using Microsoft.EntityFrameworkCore;
using ARHI_VAJAZAse2.Modeli;


namespace ARHI_VAJAZAse2.Data
{
    public class LibraryContext : DbContext
    {

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Author { get; set; } = null!;
        public DbSet<Rental> Rentals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

    }
}
