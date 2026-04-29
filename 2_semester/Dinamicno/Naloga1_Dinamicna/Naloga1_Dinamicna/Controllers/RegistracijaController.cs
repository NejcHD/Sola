using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naloga1_Dinamicna.Models;
using Naloga1_Dinamicna.Data; // To si verjetno pozabil dodati!
using Newtonsoft.Json;
using System.Linq;

namespace Naloga1_Dinamicna.Controllers
{
    public class RegistracijaController : Controller
    {
        // 1. POVEZAVA DO BAZE
        private readonly ApplicationDbContext _context;

        // 2. KONSTRUKTOR (mora biti znotraj razreda!)
        public RegistracijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 3. SEZNAM VSEH (Tukaj prebereš bazo)
        public IActionResult Seznam()
        {
            var vsi = _context.Uporabniki.ToList();
            return View(vsi);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegistracijaViewModel());
        }

        [HttpPost]
        public IActionResult Index(RegistracijaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // SHRANJEVANJE V BAZO
                _context.Uporabniki.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Seznam");
            }
            return View(model);
        }

        // UREJANJE - GET
        public IActionResult Uredi(int id)
        {
            var uporabnik = _context.Uporabniki.Find(id);
            if (uporabnik == null) return NotFound();

            var vm = new RegistracijaViewModel
            {
                Id = uporabnik.Id,
                Ime = uporabnik.Ime,
                Priimek = uporabnik.Priimek,
                Email = uporabnik.Email,
                Starost = uporabnik.Starost,
                Emso = uporabnik.Emso,
                Naslov = uporabnik.Naslov,
                Posta = uporabnik.Posta,
                PostnaStevilka = uporabnik.PostnaStevilka,
                Drzava = uporabnik.Drzava
            };
            return View(vm);
        }

        // UREJANJE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Uredi(RegistracijaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Uporabniki.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Seznam");
            }
            return View(model);
        }

        // BRISANJE
        public IActionResult Izbrisi(int id)
        {
            var u = _context.Uporabniki.Find(id);
            if (u != null)
            {
                _context.Uporabniki.Remove(u);
                _context.SaveChanges();
            }
            return RedirectToAction("Seznam");
        }
    }
}