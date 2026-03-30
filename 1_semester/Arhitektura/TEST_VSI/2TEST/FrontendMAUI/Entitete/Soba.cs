namespace FrontendMAUI.Entitete
{
    public class SobaDto
    {
        public string Stevilka { get; set; } = default!;
        public int Kapaciteta { get; set; }
        public decimal CenaNaNoc { get; set; }
    }

    public class Soba
    {
        public int Id { get; set; }
        public string Stevilka { get; set; } = default!;   // npr. "203"
        public int Kapaciteta { get; set; }                // npr. 2
        public decimal CenaNaNoc { get; set; }             // npr. 89.90
        public List<Rezervacija> Rezervacije { get; set; }

        public Soba() { }

        public Soba(string stevilka, int kapaciteta, decimal cenaNaNoc)
        {
            Stevilka = stevilka;
            Kapaciteta = kapaciteta;
            CenaNaNoc = cenaNaNoc;
        }

        public Soba(int id, string stevilka, int kapaciteta, decimal cenaNaNoc) : this(stevilka, kapaciteta, cenaNaNoc)
        {
            Id = id;
        }

        public Soba(int id, string stevilka, int kapaciteta, decimal cenaNaNoc, List<Rezervacija> rezervacije)
            : this(id, stevilka, kapaciteta, cenaNaNoc)
        {
            Rezervacije = rezervacije;
        }
    }
}
