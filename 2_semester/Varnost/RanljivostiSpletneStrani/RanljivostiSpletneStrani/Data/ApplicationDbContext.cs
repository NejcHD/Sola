using Microsoft.EntityFrameworkCore;
using RanljivostiSpletneStrani.Models;

namespace RanljivostiSpletneStrani.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "123", Email = "admin@test.com", CreditCard = "1111-2222-3333-4444" },
                new User { Id = 2, Username = "janez", Password = "geslo", Email = "janez@test.com", CreditCard = "5555-6666-7777-8888" }
            );
        }

    }
}
