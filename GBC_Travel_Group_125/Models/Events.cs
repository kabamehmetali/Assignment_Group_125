using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_125.Models 
{
    public class Events
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Event name cannot be longer than 100 characters.")]
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string? EventDescription { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime DateStart { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime DateEnd { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Ticket Price")]
        public float TicketPrice { get; set; }

        [Display(Name = "Event Image")]
        public string? EventImage { get; set; }
    }
}
