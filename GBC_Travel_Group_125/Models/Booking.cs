using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_125.Models;

// Example Booking model (adjust based on your actual model)
public class Booking
{
    public string FlightNumber { get; set; }


        [Key]
        public int BookingId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int BookingType { get; set; }
      
        public int FlightId { get; set; }  // Foreign key to Flight
        public virtual Flights Flight { get; set; }  // Navigation property

    // Assuming VehicleId as a foreign key
    [Required]
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicles Vehicle { get; set; }

    }

