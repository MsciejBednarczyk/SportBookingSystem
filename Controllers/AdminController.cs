using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportBookingSystem.Models
{
    // Rozszerzenie domyślnego użytkownika Identity – wzorzec z zajęć
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Imię")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Display(Name = "Numer telefonu")]
        public string? PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    // Model z zajęć – GET/POST z walidacją
    public class SimpleMessageModel
    {
        [Required(ErrorMessage = "Proszę podać swoje imię.")]
        [Display(Name = "Twoje imię")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Proszę wpisać wiadomość.")]
        [Display(Name = "Wiadomość")]
        public string? Message { get; set; }
    }

    // ViewModel do tworzenia rezerwacji
    public class CreateReservationViewModel
    {
        [Required]
        public int CourtId { get; set; }

        [Required(ErrorMessage = "Data jest wymagana.")]
        [Display(Name = "Data rezerwacji")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana.")]
        [Display(Name = "Godzina rozpoczęcia")]
        public string StartTimeStr { get; set; } = "08:00";

        [Required(ErrorMessage = "Godzina zakończenia jest wymagana.")]
        [Display(Name = "Godzina zakończenia")]
        public string EndTimeStr { get; set; } = "09:00";

        [Display(Name = "Uwagi")]
        [StringLength(300)]
        public string? Notes { get; set; }

        // Do wyświetlenia w formularzu
        public Court? Court { get; set; }
        public decimal CalculatedPrice { get; set; }
    }
}

