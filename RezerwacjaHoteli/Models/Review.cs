using RezerwacjaHoteli.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RezerwacjaHoteli.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Ocena jest wymagana")]
        [Range(1, 5, ErrorMessage = "Ocena musi być między 1 a 5")]
        public int Rating { get; set; }

        [StringLength(int.MaxValue)]
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Relacje
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
    }
}
