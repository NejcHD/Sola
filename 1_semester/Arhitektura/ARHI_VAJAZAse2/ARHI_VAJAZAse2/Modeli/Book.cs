using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ARHI_VAJAZAse2.Modeli
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2024)]
        public int Year { get; set; }


        [MaxLength(50)]
        public string Genra { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Language { get; set; } = "Slovenian";
    }
}
