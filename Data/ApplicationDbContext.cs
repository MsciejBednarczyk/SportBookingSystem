using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportBookingSystem.Models;

namespace SportBookingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Konfiguracja relacji Court -> Reservation
            builder.Entity<Reservation>()
                .HasOne(r => r.Court)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CourtId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji User -> Reservation
            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed danych – przykładowe korty
            builder.Entity<Court>().HasData(
                new Court
                {
                    Id = 1,
                    Name = "Kort 1 – Korty Centralne",
                    Description = "Główny kort z oświetleniem LED, nawierzchnia twarda.",
                    PricePerHour = 80m,
                    Surface = "Twarda",
                    IsActive = true,
                    IsIndoor = false
                },
                new Court
                {
                    Id = 2,
                    Name = "Kort 2 – Mączka",
                    Description = "Klasyczny kort z mączką ceglaną, idealny na dłuższe wymiany.",
                    PricePerHour = 60m,
                    Surface = "Mączka",
                    IsActive = true,
                    IsIndoor = false
                },
                new Court
                {
                    Id = 3,
                    Name = "Kort 3 – Kryty",
                    Description = "Kort kryty, dostępny przez cały rok niezależnie od pogody.",
                    PricePerHour = 100m,
                    Surface = "Twarda",
                    IsActive = true,
                    IsIndoor = true
                }
            );
        }
    }
}

