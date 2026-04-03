using Microsoft.AspNetCore.Mvc;
using Naloga1_Dinamicna.Models;

namespace Naloga1_Dinamicna.Controllers
{
    public class LetiController : Controller
    {
        public IActionResult Index()
        {

            var Leti = new List<Let>
            {
                new Let
                {
                    Id = 1,
                    StevilkaLeta =  "2177",
                    IzhodisceLokacija = "Ljubljana",
                    CiljnaLokacija = "Maribor",
                    DatumOdhoda =  new DateTime(2003,4,1),
                    DatumPrihoda = new DateTime(2003,4,2),
                    CenaNajcenejsega = 700.30,
                    CenaBusiness = 1399.39,
                },
                 new Let
                {
                    Id = 2,
                    StevilkaLeta =  "3000",
                    IzhodisceLokacija = "Ljubljana",
                    CiljnaLokacija = "Zrce",
                    DatumOdhoda =  new DateTime(2001,4,4),
                    DatumPrihoda = new DateTime(2001,4,8),
                    CenaNajcenejsega = 300.80,
                    CenaBusiness = 1199.39,
                },
                  new Let
                {
                    Id = 3,
                    StevilkaLeta =  "1223",
                    IzhodisceLokacija = "Ljubljana",
                    CiljnaLokacija = "Prag",
                    DatumOdhoda =  new DateTime(2006,5,2),
                    DatumPrihoda = new DateTime(2006,5,8),
                    CenaNajcenejsega = 300,
                    CenaBusiness = 499.39,
                },
                   new Let
                {
                    Id = 4,
                    StevilkaLeta =  "1111",
                    IzhodisceLokacija = "Ljubljana",
                    CiljnaLokacija = "Californija",
                    DatumOdhoda =  new DateTime(2001,9,11),
                    DatumPrihoda = new DateTime(2001,9,12),
                    CenaNajcenejsega = 3000,
                    CenaBusiness = 50000,
                },
            };

            return View(Leti);
        }
    }



}
