using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;



namespace Vaja4NalogaTest
{
    public class PdBaza : DbContext
    {
        public DbSet<Knjiga> VseKnjige { get; set; }
        public DbSet<Zaloznik> VsiZaložniki { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Knjigarna.db");
        }

        
        public void UstvariBazo()
        {
            this.Database.EnsureCreated();
            Console.WriteLine("Baza ustvarjena!");
        }
        
        
        
        //Dodajanje zacetnih pdoatkov da nebo baza prazna

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Zaloznik>().HasData(
            
                new Zaloznik { Id = 1, Name = "Založba Modrijan" },
                new Zaloznik { Id = 2, Name = "Mladinska Knjiga" }
            );
            
            modelBuilder.Entity<Knjiga>().HasData(
            
                new Knjiga
                {
                    Id = 1,
                    Naslov = "Potovanje na luno",
                    Avtor = "Jules Verne",
                    DatumIzdelave = new DateTime(1865, 1, 1),
                    ZaloznikId = 2 // Povezava na Mladinsko Knjigo
                }
            );
        }
        
        
        
        
        
    }
    
    
    
    
}