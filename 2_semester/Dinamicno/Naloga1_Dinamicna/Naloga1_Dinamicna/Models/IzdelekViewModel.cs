using System.ComponentModel.DataAnnotations;

namespace Naloga1_Dinamicna.Models
{
    public class IzdelekViewModel
    {
        [Key] // Ta oznaka pove bazi, da je to primarni ključ
        public int Id { get; set; }

        [Required(ErrorMessage = "Vnesite naziv izdelka")]
        [Display(Name = "Naziv izdelka")]
        public string Naziv { get; set; } // Preveri, da si tukaj popravil prejšnjo napako (ime ni isto kot razred)

        [Required]
        [Range(1, 500)]
        [Display(Name = "Količina")]
        public int Kolicina { get; set; }

        [Required]
        [Display(Name = "Cena (€)")]
        public double Cena { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum dobave")]
        public DateTime DatumDobave { get; set; }
    }
}