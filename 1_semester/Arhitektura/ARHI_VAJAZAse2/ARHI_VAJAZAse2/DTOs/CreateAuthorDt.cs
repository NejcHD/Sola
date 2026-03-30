using System.ComponentModel.DataAnnotations;

namespace ARHI_VAJAZAse2.DTOs
{
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Ime je obvezno")]
        [StringLength(50, ErrorMessage = "Ime je lahko največ 50 znakov")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priimek je obvezen")]
        [StringLength(50, ErrorMessage = "Priimek je lahko največ 50 znakov")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Država je lahko največ 50 znakov")]
        public string Country { get; set; } = string.Empty;
    }
}