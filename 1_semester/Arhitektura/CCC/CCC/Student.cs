using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCC
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(5,1000)]
        public int age { get; set; }

        [Column("DateCreated")]  // Anotacija 7: Določi ime stolpca
        public DateTime Datum { get; set; } = DateTime.Now;
    }
}
