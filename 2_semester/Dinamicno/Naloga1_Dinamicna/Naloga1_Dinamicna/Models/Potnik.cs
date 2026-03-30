namespace Naloga1_Dinamicna.Models
{
    public class Potnik
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Priimek { get; set; } = "";       
        public DateTime DatumRojstva { get; set; }
        public string StevilkaPotnega { get; set; } = "";
        public double StanjeRacuna { get; set; }
        public int SteviloLetov { get; set; }     
    }
}