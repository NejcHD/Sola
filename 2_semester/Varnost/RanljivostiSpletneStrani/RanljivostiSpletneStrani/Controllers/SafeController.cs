using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanljivostiSpletneStrani.Data;
using RanljivostiSpletneStrani.Models;

namespace RanljivostiSpletneStrani.Controllers
{
    public class SafeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SafeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

     
        //  uporabljamo LINQ, ki avtomatsko parametrizira poizvedbo.
        [HttpPost]
        public IActionResult SqlInjection(string username)
        {
            var users = _context.Users.Where(u => u.Username == username).ToList();
            ViewBag.LastQuery = "SELECT * FROM Users WHERE Username = @p0";
            return View("Index", users);
        }

     
        //  v pogledu NE uporabljamo Html.Raw(). ASP.NET avtomatsko zakodira izpis.
        public IActionResult Xss(string name)
        {
            ViewBag.UserName = name;
            return View("Index", _context.Users.ToList());
        }

        
        // preverimo, ali ima uporabnik pravico do vpogleda.
        public IActionResult UserProfile(int id)
        {
            if (id <= 0 || id > 2)
            {
                return Unauthorized("Dostop zavrnjen: nimate pravic za ogled tega profila.");
            }

            var user = _context.Users.Find(id);
            return View("Index", new List<User> { user });
        }

        
        // gesla pred prikazom maskiramo. V bazi bi morala biti hashirana.
        public IActionResult ShowPasswords()
        {
            var users = _context.Users.ToList();
            foreach (var u in users)
            {
                u.Password = "********";
            }
            return View("Index", users);
        }

        
        //uporabljamo try-catch in uporabniku ne razkrijemo sistemskih poti.
        public IActionResult CauseError()
        {
            try
            {
                throw new Exception("Sistemska napaka.");
            }
            catch
            {
                
                return View("ErrorPage");
            }
        }

       
        //  zahtevamo minimalno dolžino gesla 
        [HttpPost]
        public IActionResult ChangePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 8)
            {
                ViewBag.Error = "NAPAKA: Geslo mora imeti vsaj 8 znakov!";
                return View("Index", _context.Users.ToList());
            }

            ViewBag.PassMsg = "Geslo uspešno posodobljeno.";
            return View("Index", _context.Users.ToList());
        }

        
        //prepovemo ".." in omejimo dostop samo na varno mapo.
        public IActionResult ReadFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) || filename.Contains(".."))
            {
                ViewBag.FileError = "Dostop zavrnjen: neveljavno ime datoteke.";
                return View("Index", _context.Users.ToList());
            }

            try
            {
                // Datoteke iščemo samo znotraj strogo določene mape
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "public", filename);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.FileContent = System.IO.File.ReadAllText(path);
                }
            }
            catch { /* Napako tiho zabeležimo na strežniku */ }

            return View("Index", _context.Users.ToList());
        }

       
        //  uporabljamo seznam dovoljenih URL-jev
        public async Task<IActionResult> Proxy(string url)
        {
            var allowedUrls = new List<string> { "https://www.google.com" };

            if (!allowedUrls.Contains(url))
            {
                ViewBag.ProxyError = "Napaka: URL ni na seznamu dovoljenih virov.";
                return View("Index", _context.Users.ToList());
            }

            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync(url);
                ViewBag.ProxyContent = content;
            }
            catch { ViewBag.ProxyError = "Nalaganje vsebine ni uspelo."; }

            return View("Index", _context.Users.ToList());
        }
    }
}