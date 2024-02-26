using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_125.Models
{
    public class Rooms
    {
        [Key]
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public double Price { get; set; }
        
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? RoomImage { get; set; }

        [Required]
        public int NumOfBeds { get; set; }
    }
}