using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RezerwacjaHoteli.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny email")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(int.MaxValue)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko wymagane")]
        [StringLength(255, MinimumLength = 3)]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Wprowadź poprawny numer telefonu")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "Guest"; // "Guest", "Admin"

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }

        // Relacje
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
