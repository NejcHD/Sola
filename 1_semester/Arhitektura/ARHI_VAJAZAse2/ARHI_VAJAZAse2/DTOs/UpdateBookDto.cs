using System.ComponentModel.DataAnnotations;

namespace ARHI_VAJAZAse2.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2024)]
        public int Year { get; set; }

        [StringLength(50)]
        public string Genra { get; set; } = string.Empty;
    }
}