namespace Arhi_Vaja3.Models
{
    public class Izposoja
    {
        public int Id { get; set; }
        public DateTime DatumIzposoje { get; set; }
        public DateTime? DatumVrnitve { get; set; }
        public string ImeIzposodbe { get; set; } = string.Empty;

        
        public int IdKnjige { get; set; }
        public int IdAvtorja { get; set; }
    }
}
