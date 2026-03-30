namespace Arhi_Vaja3.Models

{
    public class Knjige
    {
        public int Id { get; set; }
        public string Naslob { get; set; } = string.Empty;
        public int DatumObjave { get; set; }
        public bool Navoljo { get; set; } = true;
    }
}