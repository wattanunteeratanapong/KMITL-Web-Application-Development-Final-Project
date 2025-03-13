using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Togeta.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public required string location { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Participants must be at least 0.")]

        public int Participants { get; set; }
        [Required]
        public required string Tag { get; set; }
        [Required]

        public required string Detail { get; set; }
        [Required]
        public required string Imagepath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required]
        public required int CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedDate { get; set; }

        public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();

    }
}
