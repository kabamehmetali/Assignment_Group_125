using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_125.Models
{
    public class Flights
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        public string? FlightNumber { get; set; }
        [Required]
        public string? Airline { get; set; }

        [Required]
        public string? FlightFrom { get; set; }

        [Required]
        public string? FlightTo { get; set; }

        [Required]
        public DateTime DepartureDateAndTime { get; set; }

        [Required]
        public DateTime ArrivalDateAndTime { get; set; }

        private int _defaultCapacity = 0; // Default value for Capacity
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Capacity must be 0 or greater")]
        public int Capacity
        {
            get { return _defaultCapacity; }
            set { _defaultCapacity = value < 0 ? 0 : value; }
        }
        [Required]
        public double Price { get; set; }

    }
}
