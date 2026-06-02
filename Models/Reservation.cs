using System.ComponentModel.DataAnnotations;

namespace SportBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int CourtId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data rezerwacji jest wymagana.")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana.")]
        [Display(Name = "Godzina rozpoczęcia")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Godzina zakończenia jest wymagana.")]
        [Display(Name = "Godzina zakończenia")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Status")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        [Display(Name = "Uwagi")]
        [StringLength(300)]
        public string? Notes { get; set; }

        [Display(Name = "Data utworzenia")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Cena wyliczana na podstawie czasu
        [Display(Name = "Cena łączna (zł)")]
        public decimal TotalPrice { get; set; }

        // Nawigacje
        public Court? Court { get; set; }
        public ApplicationUser? User { get; set; }
    }

    public enum ReservationStatus
    {
        Active,
        Cancelled,
        Completed
    }
}
