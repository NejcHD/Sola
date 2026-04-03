using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Tekma
    {
        [Key]
        public int idTekma { get; set; }
        public string? Ime_tekme { get; set; }
        public int? Leto { get; set; }
        public string? Lokacija { get; set; }
        public decimal? Latituda { get; set; }
        public decimal? Longituda { get; set; }

        public int? Tip_Tekmovanja_idTip_Tekmovanja { get; set; }
        public int? Drzava_idDrzava { get; set; }
    }
}