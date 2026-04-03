using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class Tekmovalci
    {
        [Key]
        public int? idTekmovalci { get; set; }
      
        public string? Ime_Priimek { get; set; }
       
        public string? Spol { get; set; }
        public int? Drzava_idDrzava { get; set; }

        [ForeignKey("Drzava_idDrzava")]
        public virtual Drzava? Drzava { get; set; }
    }
}