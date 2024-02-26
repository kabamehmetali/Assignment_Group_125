using System;
using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_125.Models
{
    public class Users
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)] // Consider specifying maximum length
        public string? Username { get; set; }

        [Required]
        [EmailAddress] // Validates the Email format
        [StringLength(255)] // Consider specifying maximum length
        public string? Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)] // Consider specifying maximum length and minimum length for passwords
        public string? Password { get; set; } // Consider using a secure hash instead of plain text. Look into using a library for hashing passwords, like BCrypt or ASP.NET Identity.

        [Required]
        [StringLength(255)] // Consider specifying maximum length
        public string? FirstName { get; set; }

        [StringLength(255)] // Consider specifying maximum length
        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; } // If every user must have a date of birth, it's good to not make this nullable. Otherwise, use DateTime? for optional dates.

        [Required]
        public DateTime CreatedAt { get; set; } // Consider setting a default value like DateTime.UtcNow to automatically set this field when a new user is created.
    }
}
