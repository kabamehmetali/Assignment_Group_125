using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_125.Models
{
    public class Stays
    {
        [Key]
        public int StayId { get; set; }

        [Required]
        public string? HotelName { get; set; }

        [Required]
        public int MaximumRoom { get; set; }

        // This property will store the path to the image
        public string? HotelImage { get; set; }

        [Required]
        public string? HotelLocation { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public Rooms[]? Rooms { get; set; }
    
    }
}
