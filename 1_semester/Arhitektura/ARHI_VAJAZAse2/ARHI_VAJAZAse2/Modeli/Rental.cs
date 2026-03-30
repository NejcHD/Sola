using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARHI_VAJAZAse2.Modeli
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
    }
}
