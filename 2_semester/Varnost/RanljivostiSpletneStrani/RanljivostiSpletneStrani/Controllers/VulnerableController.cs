using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanljivostiSpletneStrani.Data;
using RanljivostiSpletneStrani.Models;

namespace RanljivostiSpletneStrani.Controllers
{
    public class VulnerableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VulnerableController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

       
        // vnos neposredno združimo v niz (string) in ga izvedemo v bazi.
        [HttpPost]
        public IActionResult SqlInjection(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
            var users = _context.Users.FromSqlRaw(query).ToList();
            ViewBag.LastQuery = query;
            return View("Index", users);
        }

       
        //  vnos shranimo v viewBag brez preverjanja in ga v pogledu izpišemo kot surovo kodo
        public IActionResult Xss(string name)
        {
            ViewBag.UserName = name;
            return View("Index", _context.Users.ToList());
        }

       
        //  uporabnik ročno spremeni ID v URL-ju in vidi tuje profile.
        public IActionResult UserProfile(int id)
        {
            var user = _context.Users.Find(id);
            return View("Index", new List<User> { user });
        }

        
        //  pošljemo gesla neposredno v tabelo v berljivi obliki plaintext
        public IActionResult ShowPasswords()
        {
            return View("Index", _context.Users.ToList());
        }

        
        //  sprožimo napako, ki uporabniku razkrije poti do datotek na strežniku.
        public IActionResult CouseEroor()
        {
            throw new Exception("Kritična napaka v bazi na: C:/Users/Admin/Project/baza.db");
        }

        
        // sprejmemo karkoli (npr. geslo 1) brez preverjanja moči gesla.
        [HttpPost]
        public IActionResult ChangePassword(string newPassword)
        {
            ViewBag.PassMsg = "Geslo spremenjeno na: " + newPassword;
            // NUJNO: vrniti moraš View, sicer brskalnik ne ve, kako prikazati stran
            return View("Index", _context.Users.ToList());
        }

        
        // uporabniku dovolimo, da sam določi pot do datoteke (npr. ../appsettings.json).
        public IActionResult ReadFile(string filename)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filename);
                ViewBag.FileContent = System.IO.File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                ViewBag.FileContent = "Napaka: " + ex.Message;
            }
            return View("Index", _context.Users.ToList());
        }

       
        // naš strežnik deluje kot posrednik  in obišče katerikoli URL, ki ga poda.
        public async Task<IActionResult> Proxy(string url)
        {
            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync(url);
                ViewBag.ProxyContent = content;
            }
            catch { ViewBag.ProxyContent = "URL-ja ni mogoče naložiti."; }

            return View("Index", _context.Users.ToList());
        }
    }
}