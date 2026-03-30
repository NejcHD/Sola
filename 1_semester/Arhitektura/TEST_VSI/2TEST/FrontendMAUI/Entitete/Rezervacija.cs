namespace FrontendMAUI.Entitete
{
    public class RezervacijaDto
    {
        public DateOnly Od { get; set; }
        public DateOnly Do { get; set; }
        public string ImeGosta { get; set; }
        public string? Opomba { get; set; }
        public int SobaId { get; set; }
    }
    public class Rezervacija
    {
        public int Id { get; set; }
        public DateOnly Od { get; set; }
        public DateOnly Do { get; set; }

        public string ImeGosta { get; set; }
        public string? Opomba { get; set; }
        public int SobaId { get; set; }
        public Soba Soba { get; set; }

        public Rezervacija()
        {
        }

        public Rezervacija(DateOnly od, DateOnly do_, string imeGosta, int sobaId)
        {
            Od = od;
            Do = do_;
            ImeGosta = imeGosta;
            SobaId = sobaId;
        }

        public Rezervacija(DateOnly od, DateOnly do_, string imeGosta, string? opomba, int sobaId)
            : this(od, do_, imeGosta, sobaId)
        {
            Opomba = opomba;
        }

        public Rezervacija(int id, DateOnly od, DateOnly do_, string imeGosta, int sobaId)
            : this(od, do_, imeGosta, sobaId)
        {
            Id = id;
        }

        public Rezervacija(int id, DateOnly od, DateOnly do_, string imeGosta, string? opomba, int sobaId, Soba soba)
            : this(id, od, do_, imeGosta, sobaId)
        {
            Opomba = opomba;
            Soba = soba;
        }
    }

}
