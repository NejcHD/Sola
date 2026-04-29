using Microsoft.EntityFrameworkCore;
using Naloga1_Dinamicna.Models;
using System.Collections.Generic;

namespace Naloga1_Dinamicna.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabela za uporabnike (brez gesel)
        public DbSet<Uporabnik> Uporabniki { get; set; }

        // Tabela za izdelke
        public DbSet<IzdelekViewModel> Izdelki { get; set; }
    }
}