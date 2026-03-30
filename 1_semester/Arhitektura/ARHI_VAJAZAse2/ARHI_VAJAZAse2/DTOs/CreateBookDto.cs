using System.ComponentModel.DataAnnotations;

namespace ARHI_VAJAZAse2.DTOs
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Naslov je obvezen")]
        [StringLength(100, ErrorMessage = "Naslov je lahko največ 100 znakov")]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2024, ErrorMessage = "Leto mora biti med 1000 in 2024")]
        public int Year { get; set; }

        [StringLength(50, ErrorMessage = "Žanr je lahko največ 50 znakov")]
        public string Genra { get; set; } = string.Empty;
    }
}