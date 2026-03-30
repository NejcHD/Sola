using System.ComponentModel.DataAnnotations;

namespace Backend.Entitete
{
    public class Nabiralnik
    {
        [Key]
        public string Email { get; set; }
        public List<Sporocilo> PrejetaSporocila { get; set; }
        public List<Sporocilo> PoslanaSporocila { get; set; }

        public Boolean Aktiven { get; set; } = true;


    }
}
