using System;
using System.ComponentModel.DataAnnotations;

namespace Naloga1_Dinamicna.Models
{
    public class RegistracijaViewModel
    {
        // --- 1. KORAK: Osebni podatki ---
        
        public string Ime { get; set; }

        
        public string Priimek { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumRojstva { get; set; }

        public string Emso { get; set; }

        public int Starost { get; set; } 


        // --- 2. KORAK: Naslov ---
        public string Naslov { get; set; }
        public string PostnaStevilka { get; set; }
        public string Posta { get; set; }
        public string Drzava { get; set; }


        // --- 3. KORAK: Dostopni podatki ---
        [Required]
        [EmailAddress(ErrorMessage = "Vnesite veljaven e-naslov")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Geslo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Geslo", ErrorMessage = "Gesli se ne ujemata")]
        public string PotrditevGesla { get; set; }
    }
}