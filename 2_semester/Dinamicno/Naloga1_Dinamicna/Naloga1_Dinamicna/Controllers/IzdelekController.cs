using Microsoft.AspNetCore.Mvc;
using Naloga1_Dinamicna.Models;
using Naloga1_Dinamicna.Data; // Nujno za dostop do baze
using System.Linq;

namespace Naloga1_Dinamicna.Controllers
{
    public class IzdelekController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Konstruktor poveže bazo s kontrolerjem
        public IzdelekController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. IZPIS VSEH IZDELKOV (Read)
        public IActionResult Seznam()
        {
            var vsiIzdelki = _context.Izdelki.ToList();
            return View(vsiIzdelki);
        }

        // 2. DODAJANJE - OBRAZEC (Create GET)
        [HttpGet]
        public IActionResult Index()
        {
            return View(new IzdelekViewModel());
        }

        // 2. DODAJANJE - SHRANJEVANJE (Create POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IzdelekViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Izdelki.Add(model);
                _context.SaveChanges(); // Shranimo v SQL bazo
                return RedirectToAction("Seznam");
            }
            return View(model);
        }

        // 3. UREJANJE - OBRAZEC (Update GET)
        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var izdelek = _context.Izdelki.Find(id);
            if (izdelek == null) return NotFound();
            return View(izdelek);
        }

        // 3. UREJANJE - POSODOBITEV (Update POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Uredi(IzdelekViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Izdelki.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Seznam");
            }
            return View(model);
        }

        // 4. BRISANJE (Delete)
        public IActionResult Izbrisi(int id)
        {
            var izdelek = _context.Izdelki.Find(id);
            if (izdelek != null)
            {
                _context.Izdelki.Remove(izdelek);
                _context.SaveChanges();
            }
            return RedirectToAction("Seznam");
        }
    }
}