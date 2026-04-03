using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class TipTekmovanja
    {
        [Key]
        public int idTip_Tekmovanja { get; set; }
        public string? Tip { get; set; }
    }
}