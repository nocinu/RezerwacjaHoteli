using RezerwacjaHoteli.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RezerwacjaHoteli.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Numer pokoju jest wymagany")]
        [StringLength(10)]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Typ pokoju jest wymagany")]
        [StringLength(50)]
        public string RoomType { get; set; } // "Single", "Double", "Suite"

        [Required(ErrorMessage = "Cena za noc jest wymagana")]
        [Range(0.01, 99999.99)]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PricePerNight { get; set; }

        [Required(ErrorMessage = "Maksymalna liczba gości jest wymagana")]
        [Range(1, 10)]
        public int MaxGuests { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Available"; // "Available", "Booked", "Maintenance"

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Relacje
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
