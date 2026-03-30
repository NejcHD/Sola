using Microsoft.AspNetCore.Mvc;
using Naloga1_Dinamicna.Models;

namespace Naloga1_Dinamicna.Controllers
{
    public class PotnikiController : Controller
    {
        public IActionResult Index()
        {
            var potniki = new List<Potnik>
            {
                new Potnik
                {
                    Id = 1,
                    Ime = "Janez",
                    Priimek = "Kovac",
                    DatumRojstva = new DateTime(1990,5,8),
                    StanjeRacuna = 5000,
            
                },
                new Potnik
                {
                    Id = 2,
                    Ime = "Rok",
                    Priimek = "Kogovscek",
                    DatumRojstva = new DateTime(1690, 6,9),
                    StanjeRacuna = 2,

                },
                new Potnik
                {
                    Id = 3,
                    Ime = "Filip",
                    Priimek = "Vertovec",
                    DatumRojstva = new DateTime(2000, 3,2),
                    StanjeRacuna = 827,
                }
            };
            return View(potniki);
        }
    }
}
