using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBookingSystem.Data;
using SportBookingSystem.Models;

namespace SportBookingSystem.Controllers
{
    public class CourtsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Courts – lista wszystkich kortów (publiczna)
        public async Task<IActionResult> Index()
        {
            var courts = await _context.Courts
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(courts);
        }

        // GET: /Courts/Details/5 – szczegóły kortu
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var court = await _context.Courts
                .FirstOrDefaultAsync(c => c.Id == id);

            if (court == null) return NotFound();

            return View(court);
        }
    }
}

