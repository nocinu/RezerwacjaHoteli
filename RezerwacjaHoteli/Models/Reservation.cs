using RezerwacjaHoteli.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RezerwacjaHoteli.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Data zaznaczenia jest wymagana")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Liczba gości jest wymagana")]
        [Range(1, 10)]
        public int NumberOfGuests { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"

        [StringLength(int.MaxValue)]
        public string SpecialRequests { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Relacje
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}
