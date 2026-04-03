using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Drzava
    {
        [Key]
        public int idDrzava { get; set; }
        public string? Ime_Drzave { get; set; }
        public string? Koda_Drzave { get; set; }
    }
}