using Microsoft.EntityFrameworkCore;

namespace BBB
{
    public class PodatkiPB : DbContext
    {
        public DbSet<Student> VsiStudentje { get; set; } // Seznam Studentov

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=students.db");
        }

        
        public void NapolniBazo()
        {
            Database.EnsureCreated(); // Ustvari bazo, če ne obstaja

            if (!VsiStudentje.Any())
            {
                VsiStudentje.AddRange(
                    new Student { Name = "Janez", age = 15 },
                    new Student { Name = "Rok", age = 16 },
                    new Student { Name = "Vinko", age = 19 },
                    new Student { Name = "Filip", age = 20 }
                );

                SaveChanges();
                Console.WriteLine("Testni podatki dodani v bazo!");
            }
        }
    } 
} 