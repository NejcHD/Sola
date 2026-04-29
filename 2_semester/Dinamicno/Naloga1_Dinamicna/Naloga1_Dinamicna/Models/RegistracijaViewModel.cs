using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Nujno dodaj to za [NotMapped]

namespace Naloga1_Dinamicna.Models
{
    // 1. Dodaj ": Uporabnik" -> To pomeni, da RegistracijaViewModel dobi vsa polja od Uporabnika
    // 2. Dodaj [NotMapped] -> To pove bazi, naj tega razreda ne shranjuje (ker ne želimo gesel v bazi)
    [NotMapped]
    public class RegistracijaViewModel : Uporabnik
    {
        // Tu ne rabiš več Imena, Priimka, EMŠO-ja itd., ker jih dobiš od "Uporabnika"
        // Zapišeš samo tisto, kar je DODATNO (gesla)

        [Required(ErrorMessage = "Geslo je obvezno")]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public string Geslo { get; set; }

        [Required(ErrorMessage = "Potrditev gesla je obvezna")]
        [DataType(DataType.Password)]
        [Compare("Geslo", ErrorMessage = "Gesli se ne ujemata")]
        [Display(Name = "Potrdi geslo")]
        public string PotrditevGesla { get; set; }
    }

    // TALE RAZRED GRE V BAZO
    public class Uporabnik
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ime")]
        [Required]
        public string Ime { get; set; }

        [Display(Name = "Priimek")]
        [Required]
        public string Priimek { get; set; }

        [Display(Name = "Datum rojstva")]
        [DataType(DataType.Date)]
        public DateTime DatumRojstva { get; set; }

        [Display(Name = "Starost")]
        public int Starost { get; set; }

        [Display(Name = "EMŠO")]
        [Emso]
        public string Emso { get; set; }

        [Display(Name = "Naslov")]
        public string Naslov { get; set; }

        [Display(Name = "Poštna Številka")]
        public string PostnaStevilka { get; set; }

        [Display(Name = "Pošta")]
        public string Posta { get; set; }

        [Display(Name = "Država")]
        public string Drzava { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Vnesite veljaven e-naslov")]
        [Display(Name = "E-pošta")]
        public string Email { get; set; }
    }
}