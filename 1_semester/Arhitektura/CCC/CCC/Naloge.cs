using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCC
{
    [Table("Naloga")]
    public class Naloga
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(150)]
        public string Opis { get; set; } // Opis naloge (npr. 'Projekt I')

        public bool JeKoncana { get; set; } = false; // Stanje naloge

        // Ključ za povezavo s tabelo Student
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public Student Student { get; set; } // Navigacijska lastnost
    }
}