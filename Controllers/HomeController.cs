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

        // Strona g³ówna
        public async Task<IActionResult> Index()
        {
             
            var courts = await _context.Courts
                .Where(c => c.IsActive)
                .Take(3)
                .ToListAsync();

            return View(courts);
        }



         
        [HttpGet]
        public IActionResult SimplePage(string? name, string? message)
        {
            
            var model = new SimpleMessageModel
            {
                Name = name,
                Message = message
            };
            return View(model);
        }


[HttpPost]
        public async Task<IActionResult> SimplePage(SimpleMessageModel model)
        {
            if (ModelState.IsValid)
            {
                
                var newMessage = new ContactMessage
                {
                    Name = model.Name,
                    Message = model.Message
                };

              
                _context.ContactMessages.Add(newMessage);
                await _context.SaveChangesAsync();

                
                ViewData["SuccessMessage"] = $"Czeœæ {model.Name}! Twoja wiadomoœæ zosta³a wys³ana i zapisana w systemie.";

              
                return View(new SimpleMessageModel());
            }

       
            return View(model);
        }

        
        public IActionResult About()
        {
            return View();
        }

   
        public IActionResult Contact()
        {
            return View();
        }

      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}


