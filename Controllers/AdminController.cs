using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBookingSystem.Data;
using SportBookingSystem.Models;

namespace SportBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCourts = await _context.Courts.CountAsync();
            ViewBag.ActiveReservations = await _context.Reservations
                .CountAsync(r => r.Status == ReservationStatus.Active);
            ViewBag.TotalReservations = await _context.Reservations.CountAsync();
            ViewBag.Revenue = await _context.Reservations
                .Where(r => r.Status != ReservationStatus.Cancelled)
                .SumAsync(r => r.TotalPrice);
            return View();
        }

        public async Task<IActionResult> Courts()
        {
            var courts = await _context.Courts.OrderBy(c => c.Name).ToListAsync();
            return View(courts);
        }

        [HttpGet]
        public IActionResult CourtCreate()
        {
            return View(new Court());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourtCreate(Court court)
        {
            if (ModelState.IsValid)
            {
                _context.Courts.Add(court);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Kort \"{court.Name}\" został dodany.";
                return RedirectToAction(nameof(Courts));
            }
            return View(court);
        }

        [HttpGet]
        public async Task<IActionResult> CourtEdit(int? id)
        {
            if (id == null) return NotFound();
            var court = await _context.Courts.FindAsync(id);
            if (court == null) return NotFound();
            return View(court);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourtEdit(int id, Court court)
        {
            if (id != court.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(court);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Kort \"{court.Name}\" został zaktualizowany.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Courts.AnyAsync(c => c.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Courts));
            }
            return View(court);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourtDelete(int id)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court == null) return NotFound();
            _context.Courts.Remove(court);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Kort \"{court.Name}\" został usunięty.";
            return RedirectToAction(nameof(Courts));
        }

        public async Task<IActionResult> Reservations()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationDelete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Rezerwacja została usunięta.";
            return RedirectToAction(nameof(Reservations));
        }
    }
}