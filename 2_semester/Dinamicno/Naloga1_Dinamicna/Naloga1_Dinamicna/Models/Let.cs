namespace Naloga1_Dinamicna.Models
{
    public class Let
    {
        public int Id { get; set; }
        public string StevilkaLeta { get; set; } = "";
        public string IzhodisceLokacija { get; set; } = "";
        public string CiljnaLokacija { get; set; } = "";
        public DateTime DatumOdhoda { get; set; }
        public DateTime DatumPrihoda { get; set; }
        public double CenaNajcenejsega { get; set; }
        public double CenaBusiness { get; set; }
        public int StSedezev { get; set; }
        public int ProstihSedezev { get; set; }
        public string TipLetala { get; set; } = "";
        public string Status { get; set; } = "";
    }
}