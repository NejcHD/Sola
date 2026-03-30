using Microsoft.AspNetCore.Mvc;
using Naloga1_Dinamicna.Models;
namespace Naloga1_Dinamicna.Controllers
{
    public class RezervacijeController : Controller
    {
        public IActionResult Index()
        {
            var rezervacije = new List<Rezervacija>
            {
                new Rezervacija
                {
                    Id = 1,
                    StevilkaRezervacije = "RES-001",
                    PotnikId = 1,
                    PotnikIme = "Janez Kovac",
                    LetId = 1,
                    StevilkaLeta = "2177",
                    DatumRezervacije = new DateTime(2003, 3, 15),
                    RazredSedeza = "Economy",
                    SteviloCenSedezev = 1,
                    CenaPoSedez = 700.30,
                    SkupnasCena = 700.30,
                    Status = "Potrjena",
                    PlacanoPrtljaga = true,
                    TezaPrtljage = 23.5
                },
                new Rezervacija
                {
                    Id = 2,
                    StevilkaRezervacije = "RES-002",
                    PotnikId = 2,
                    PotnikIme = "Rok Kogovsek",
                    LetId = 2,
                    StevilkaLeta = "3000",
                    DatumRezervacije = new DateTime(2001, 3, 1),
                    RazredSedeza = "Business",
                    SteviloCenSedezev = 2,
                    CenaPoSedez = 599.70,
                    SkupnasCena = 1199.39,
                    Status = "Potrjena",
                    PlacanoPrtljaga = false,
                    TezaPrtljage = 0
                },
                new Rezervacija
                {
                    Id = 3,
                    StevilkaRezervacije = "RES-003",
                    PotnikId = 3,
                    PotnikIme = "Filip Vertovec",
                    LetId = 3,
                    StevilkaLeta = "1223",
                    DatumRezervacije = new DateTime(2006, 4, 10),
                    RazredSedeza = "Economy",
                    SteviloCenSedezev = 1,
                    CenaPoSedez = 300.00,
                    SkupnasCena = 300.00,
                    Status = "Cakajoca",
                    PlacanoPrtljaga = true,
                    TezaPrtljage = 15.0
                }
            };
            return View(rezervacije);
        }
    }
}