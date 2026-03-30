using Arhi_Vaja3.Models;

namespace Arhi_Vaja3.Data
{
    public static class DataContext
    {
        public static List<Knjige> VseKnjige = new()
        {
            new Knjige { Id = 1, Naslob = "Prvi spopad", DatumObjave = 2020, Navoljo = true },
            new Knjige { Id = 2, Naslob = "Drugi vpogled", DatumObjave = 2021, Navoljo = false },
            new Knjige { Id = 3, Naslob = "Tretja priložnost", DatumObjave = 2022, Navoljo = true }
        };

        public static List<Pisec> VsiAvtorji = new()
        {
            new Pisec { Id = 1, Ime = "Janez", Priimek = "Novak", Drzavlanjstvo = "Slovenska", Rojstvo = new DateTime(1980, 5, 15) },
            new Pisec { Id = 2, Ime = "Ana", Priimek = "Kovač", Drzavlanjstvo = "Slovenska", Rojstvo = new DateTime(1975, 8, 22) },
            new Pisec { Id = 3, Ime = "Marko", Priimek = "Petek", Drzavlanjstvo = "Slovenska", Rojstvo = new DateTime(1990, 3, 10) }
        };

        public static List<Izposoja> VseIzposoje = new()
        {
            new Izposoja { Id = 1, IdKnjige = 1, IdAvtorja = 1, DatumIzposoje = new DateTime(2024, 1, 15), DatumVrnitve = null, ImeIzposodbe = "Peter Kralj" },
            new Izposoja { Id = 2, IdKnjige = 2, IdAvtorja = 2, DatumIzposoje = new DateTime(2024, 1, 10), DatumVrnitve = new DateTime(2024, 1, 20), ImeIzposodbe = "Maja Sever" }
        };
    }
}