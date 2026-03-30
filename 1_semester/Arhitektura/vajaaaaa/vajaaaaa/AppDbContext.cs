using System.Collections.Generic;
using vajaaaaa.Podatki;

namespace vajaaaaa
{
    public class AppDbContext
    {

        public DbSet<Student> Students { get; set; }
    }
}
