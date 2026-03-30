using System.ComponentModel.DataAnnotations.Schema;

namespace AIS_16_10_3
{
    public class Kosarica
    {
        
     

        public int Id { get; set; }

        public List<Artikel> SeznamArtiklov { get; set; }


        [NotMapped]
        public double Znesek
        {
            get { return SeznamArtiklov?.Sum(a => (double)a.Cena) ?? 0; }
        }


    }
}
