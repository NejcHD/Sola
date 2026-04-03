using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class Rezultati
    {
        [Key]
        public int idRezultata { get; set; }
        public int? Stevilka { get; set; }
        public string? Cas_Plavanja { get; set; }
        public string? Cas_Kolesarjenja { get; set; }
        public string? Cas_Teka { get; set; }
        public string? Skupni_cas { get; set; } 
        public int? Uvrstitev { get; set; }
        public string? Uvrstitev_spol { get; set; }
        public string? Uvrstitev_Kategorija { get; set; }
        public string? Kategorija { get; set; }

        public int? Tekma_idTekma { get; set; }
        [ForeignKey("Tekma_idTekma")]
        public virtual Tekma? Tekma { get; set; }


        public int? Tip_Tekmovanja_idTip_Tekmovanja { get; set; }
        [ForeignKey("Tip_Tekmovanja_idTip_Tekmovanja")]
        public virtual TipTekmovanja? TipTekmovanja { get; set; }


        public int? Tekmovalci_idTekmovalci { get; set; }

        [ForeignKey("Tekmovalci_idTekmovalci")] // Povežemo številko s spodnjim objektom
        public virtual Tekmovalci? Tekmovalci { get; set; }
    }
}