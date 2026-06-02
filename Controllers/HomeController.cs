using Microsoft.AspNetCore.Mvc;
using SportBookingSystem.Data;
using SportBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace SportBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Strona g³ówna
        public async Task<IActionResult> Index()
        {
            // Pobieramy aktywne korty do wyœwietlenia na stronie g³ównej
            var courts = await _context.Courts
                .Where(c => c.IsActive)
                .Take(3)
                .ToListAsync();

            return View(courts);
        }

        // ============================================================
        // PRZYK£AD Z ZAJÊÆ: SimplePage – GET i POST z walidacj¹ modelu
        // ============================================================

        // GET /Home/SimplePage?name=Jan&message=Test
        [HttpGet]
        public IActionResult SimplePage(string? name, string? message)
        {
            // Parametryczna metoda GET – dane z URL ustawiaj¹ domyœlne wartoœci
            var model = new SimpleMessageModel
            {
                Name = name,
                Message = message
            };
            return View(model);
        }

        // POST /Home/SimplePage
        [HttpPost]
        public IActionResult SimplePage(SimpleMessageModel model)
        {
            if (ModelState.IsValid)
            {
                ViewData["SuccessMessage"] = $"Czeœæ {model.Name}! Twoja wiadomoœæ: '{model.Message}' zosta³a odebrana.";
                return View(new SimpleMessageModel());
            }

            // Jeœli walidacja nie przejdzie – wracamy z b³êdami
            return View(model);
        }

        // GET: Strona "O nas"
        public IActionResult About()
        {
            return View();
        }

        // GET: Strona kontaktowa
        public IActionResult Contact()
        {
            return View();
        }

        // Obs³uga b³êdów
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}


