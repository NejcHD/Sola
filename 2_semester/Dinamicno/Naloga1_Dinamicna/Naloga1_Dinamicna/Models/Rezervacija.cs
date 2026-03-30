namespace Naloga1_Dinamicna.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public string StevilkaRezervacije { get; set; } = "";
        public int PotnikId { get; set; }
        public string PotnikIme { get; set; } = "";
        public int LetId { get; set; }
        public string StevilkaLeta { get; set; } = "";
        public DateTime DatumRezervacije { get; set; }
        public string RazredSedeza { get; set; } = "";
        public int SteviloCenSedezev { get; set; }
        public double CenaPoSedez { get; set; }
        public double SkupnasCena { get; set; }
        public string Status { get; set; } = "";
        public bool PlacanoPrtljaga { get; set; }
        public double TezaPrtljage { get; set; }
    }
}