using Microsoft.AspNetCore.Mvc;
using Naloga1_Dinamicna.Models;
using Newtonsoft.Json; // Če nimaš te knjižnice, jo bomo dodali kasneje

namespace Naloga1_Dinamicna.Controllers
{
    public class RegistracijaController : Controller
    {
        // 1. KORAK: Prikaz obrazca (GET)
        [HttpGet]
        public IActionResult Korak1()
        {
            return View(new RegistracijaViewModel());   // pokaze model za vpisat podatke
        }

        // 1. KORAK: Sprejem podatkov in preusmeritev (POST)
        [HttpPost]
        public IActionResult Korak1(RegistracijaViewModel model)
        {

            TempData["RegistracijaPodatki"] = JsonConvert.SerializeObject(model);  //shrani pdoatke v model
            return RedirectToAction("Korak2");
        }





        // 2. KORAK: Prikaz obrazca za naslov (GET)
        [HttpGet]
        public IActionResult Korak2()
        {
            var podatkiString = TempData["RegistracijaPodatki"] as string;                 // prebere shranjen podatke iz tempdata
            if (string.IsNullOrEmpty(podatkiString)) return RedirectToAction("Korak1");

            TempData.Keep("RegistracijaPodatki"); // preberemo ter ukazemo da ostane ker cene se zbrise

            var model = JsonConvert.DeserializeObject<RegistracijaViewModel>(podatkiString);  // pretvori iz json v c# model za kasneje
            return View(model);
        }

        // 2. KORAK: Sprejem naslova (POST)
        [HttpPost]
        public IActionResult Korak2(RegistracijaViewModel model)
        {
            var stariPodatkiString = TempData["RegistracijaPodatki"] as string;
            var stariPodatki = JsonConvert.DeserializeObject<RegistracijaViewModel>(stariPodatkiString);  // pretvori v model

            // Dodamo nove podatke k starim
            stariPodatki.Naslov = model.Naslov;
            stariPodatki.PostnaStevilka = model.PostnaStevilka;
            stariPodatki.Posta = model.Posta;
            stariPodatki.Drzava = model.Drzava;
            // pripise starim podatkom nove

            TempData["RegistracijaPodatki"] = JsonConvert.SerializeObject(stariPodatki);  // shrani spremenjeno v json
            return RedirectToAction("Korak3");


        }








        // 3. KORAK: Prikaz obrazca za Email in Geslo (GET)
        [HttpGet]
        public IActionResult Korak3()
        {
            var podatkiString = TempData["RegistracijaPodatki"] as string;
            if (string.IsNullOrEmpty(podatkiString)) return RedirectToAction("Korak1");

            TempData.Keep("RegistracijaPodatki");
            return View(new RegistracijaViewModel()); // Lahko pošlješ tudi prazen model
        }

        // 3. KORAK: Sprejem Emaila in Gesla (POST)
        [HttpPost]
        public IActionResult Korak3(RegistracijaViewModel model)
        {
            var stariPodatkiString = TempData["RegistracijaPodatki"] as string;
            var stariPodatki = JsonConvert.DeserializeObject<RegistracijaViewModel>(stariPodatkiString);


            if (model.Geslo != model.PotrditevGesla)
            {
                // Ustvarimo ročno napako, ki se bo izpisala v View-ju
                ViewBag.NapakaGeslo = "Gesli se ne ujemata!";

                // Obdržimo kovček za naslednji poskus
                TempData.Keep("RegistracijaPodatki");

                return View(model); // Ostaneš na strani Korak3
            }


            // Dodamo email in geslo
            stariPodatki.Email = model.Email;
            stariPodatki.Geslo = model.Geslo;
            stariPodatki.PotrditevGesla = model.PotrditevGesla;


            TempData["RegistracijaPodatki"] = JsonConvert.SerializeObject(stariPodatki);   // isto kot 2 korak postopek
            return RedirectToAction("Potrditev");
        }






        // 4. KORAK: Končni izpis (GET)
        [HttpGet]
        public IActionResult Potrditev()
        {
            var podatkiString = TempData["RegistracijaPodatki"] as string;
            if (string.IsNullOrEmpty(podatkiString)) return RedirectToAction("Korak1");

            var končniModel = JsonConvert.DeserializeObject<RegistracijaViewModel>(podatkiString);
            return View(končniModel);
        }
    }
}