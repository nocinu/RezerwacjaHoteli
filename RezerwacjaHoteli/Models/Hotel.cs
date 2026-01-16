using RezerwacjaHoteli.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RezerwacjaHoteli.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa hotelu jest wymagana")]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        [StringLength(500)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        [StringLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "Ocena jest wymagana")]
        [Range(1, 5, ErrorMessage = "Ocena musi być między 1 a 5")]
        public int StarRating { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Email kontaktowy jest wymagany")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Relacje
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
