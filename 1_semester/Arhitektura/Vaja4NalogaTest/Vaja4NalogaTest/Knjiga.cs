namespace Vaja4NalogaTest
{
    public class Knjiga
    {
        public int Id {get; set; }
        public string Naslov {get; set;} 
        public string Avtor  {get; set;}
        public DateTime DatumIzdelave { get; set; }
        
        public int ZaloznikId { get; set; }  // tuji kljuc 
        public Zaloznik Zaloznik { get; set; }
    }
}