using System.ComponentModel.DataAnnotations;

namespace ARHI_VAJAZAse2.DTOs
{
    public class CreateRentalDto
    {
        [Required(ErrorMessage = "Datum izposoje je obvezen")]
        public DateTime RentalDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "BookId je obvezen")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId mora biti veljaven")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "AuthorId je obvezen")]
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId mora biti veljaven")]
        public int AuthorId { get; set; }
    }
}