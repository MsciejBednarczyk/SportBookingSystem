using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportBookingSystem.Models
{
    public class Court
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kortu jest wymagana.")]
        [Display(Name = "Nazwa kortu")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cena za godzinę jest wymagana.")]
        [Display(Name = "Cena za godzinę (zł)")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000, ErrorMessage = "Cena musi być między 1 a 10000 zł.")]
        public decimal PricePerHour { get; set; }

        [Display(Name = "Nawierzchnia")]
        public string? Surface { get; set; }  // np. Twarda, Mączka, Trawa

        [Display(Name = "Aktywny")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kryty")]
        public bool IsIndoor { get; set; } = false;

        // Nawigacja
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

