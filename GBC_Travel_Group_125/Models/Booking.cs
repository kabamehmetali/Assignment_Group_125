using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_125.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public string ServiceType { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicles Vehicle { get; set; }  // Navigation property for vehicle
    }
}
