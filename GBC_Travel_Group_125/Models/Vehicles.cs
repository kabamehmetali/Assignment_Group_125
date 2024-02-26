using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_125.Models
{
    public class Vehicles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "Vehicle ID")] 
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Vehicle name is required.")]
        [StringLength(255, ErrorMessage = "Vehicle name cannot be longer than 255 characters.")]
        [Display(Name = "Vehicle Name")]
        public string? VehicleName { get; set; }

        [Required(ErrorMessage = "Vehicle type is required.")]
        [StringLength(100, ErrorMessage = "Vehicle type cannot be longer than 100 characters.")]
        [Display(Name = "Vehicle Type")] 
        public string? VehicleType { get; set; }

        [Required(ErrorMessage = "Vehicle description is required.")]
        [StringLength(500, ErrorMessage = "Vehicle description cannot be longer than 500 characters.")]
        [Display(Name = "Vehicle Description")]
        public string? VehicleDescription { get; set; }

        
        [Display(Name = "Vehicle Image URL")] 
        public string? VehicleImage { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(255, ErrorMessage = "Location cannot be longer than 255 characters.")]
        [Display(Name = "Location")] 
        public string? Location { get; set; }


        [Display(Name = "Phone Number")] 
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(100, ErrorMessage = "Model cannot be longer than 100 characters.")]
        [Display(Name = "Model")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        [StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters.")]
        [Display(Name = "Color")] 
        public string? Color { get; set; }

        [Required(ErrorMessage = "Max capacity is required.")]
        [Range(1, 100, ErrorMessage = "Max capacity must be between 1 and 100.")]
        [Display(Name = "Max Capacity")] 
        public int MaxCapacity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Availability is required.")]
        [Display(Name = "Availability")] 
        public bool Availability { get; set; }

 
    }
}
