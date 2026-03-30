using Microsoft.EntityFrameworkCore;

namespace CCC
{
    public class PodatkiPB : DbContext
    {
        public DbSet<Student> VsiStudentje { get; set; } // Seznam Studentov
        public DbSet<Naloga> VseNaloge { get; set; }    //seznam nalog
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=students.db");
        }






        public void NapolniBazo()
        {
            Database.EnsureCreated(); // Ustvari bazo in tabele

            if (!VsiStudentje.Any())
            {
                // ... obstoječi podatki ...
                VsiStudentje.AddRange(
                    new Student { Name = "Janez", age = 15 },
                    new Student { Name = "Rok", age = 16 },
                    new Student { Name = "Vinko", age = 19 },
                    new Student { Name = "Filip", age = 20 }
                );
                SaveChanges();
            }

            // NOVO: Dodajanje začetnih nalog, če ne obstajajo
            if (!VseNaloge.Any())
            {
                var janez = VsiStudentje.FirstOrDefault(s => s.Name == "Janez");
                var rok = VsiStudentje.FirstOrDefault(s => s.Name == "Rok");

                if (janez != null && rok != null)
                {
                    VseNaloge.AddRange(
                        new Naloga { Opis = "Priprava seminara", JeKoncana = false, StudentID = janez.ID },
                        new Naloga { Opis = "Izdelava spletne strani", JeKoncana = false, StudentID = rok.ID }
                    );
                    SaveChanges();
                    Console.WriteLine("Testni podatki za Naloge dodani v bazo!");
                }
            }
        }
    } 
} 