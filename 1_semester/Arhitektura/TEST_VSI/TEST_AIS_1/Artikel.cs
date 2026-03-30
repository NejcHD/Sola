using System.ComponentModel.DataAnnotations;

namespace AIS_16_10_3
{
    public class Artikel
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }

        public Artikel()
        {
            Naziv = string.Empty;
            Cena = 0;
        }

        public Artikel(string naziv, decimal cena)
        {
            Naziv = naziv;
            Cena = cena;
        }

        public Artikel(int id, string naziv, decimal cena) : this(naziv, cena)
        {
            Id = id;
        }

        
    }

}
