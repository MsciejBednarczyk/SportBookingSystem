using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBookingSystem.Data;
using SportBookingSystem.Models;

namespace SportBookingSystem.Controllers
{
    [Authorize] 
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var reservations = await _context.Reservations
                .Include(r => r.Court)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }

       
        [HttpGet]
        public async Task<IActionResult> Create(int courtId)
        {
            var court = await _context.Courts.FindAsync(courtId);
            if (court == null || !court.IsActive)
                return NotFound();

            var model = new CreateReservationViewModel
            {
                CourtId = courtId,
                Court = court,
                ReservationDate = DateTime.Today.AddDays(1)
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
           
            model.Court = await _context.Courts.FindAsync(model.CourtId);

            if (!ModelState.IsValid)
                return View(model);

           
            if (!TimeSpan.TryParse(model.StartTimeStr, out var startTime) ||
                !TimeSpan.TryParse(model.EndTimeStr, out var endTime))
            {
                ModelState.AddModelError("", "Nieprawidłowy format godziny.");
                return View(model);
            }

            if (endTime <= startTime)
            {
                ModelState.AddModelError("", "Godzina zakończenia musi być późniejsza niż rozpoczęcia.");
                return View(model);
            }

            
            bool isCollision = await _context.Reservations.AnyAsync(r =>
                r.CourtId == model.CourtId &&
                r.ReservationDate.Date == model.ReservationDate.Date &&
                r.Status == ReservationStatus.Active &&
                r.StartTime < endTime &&
                r.EndTime > startTime);

            if (isCollision)
            {
                ModelState.AddModelError("", "Wybrany termin jest już zajęty. Wybierz inny.");
                return View(model);
            }

            var court = model.Court!;
            var hours = (decimal)(endTime - startTime).TotalHours;

            var reservation = new Reservation
            {
                CourtId = model.CourtId,
                UserId = _userManager.GetUserId(User)!,
                ReservationDate = model.ReservationDate,
                StartTime = startTime,
                EndTime = endTime,
                Notes = model.Notes,
                TotalPrice = court.PricePerHour * hours,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.Now
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Rezerwacja na korcie \"{court.Name}\" w dniu {model.ReservationDate:dd.MM.yyyy} została pomyślnie złożona!";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reservation == null) return NotFound();

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rezerwacja została anulowana.";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int courtId, DateTime date)
        {
            var reservations = await _context.Reservations
                .Where(r => r.CourtId == courtId &&
                            r.ReservationDate.Date == date.Date &&
                            r.Status == ReservationStatus.Active)
                .Select(r => new { r.StartTime, r.EndTime })
                .ToListAsync();

            return Json(reservations);
        }
    }
}

