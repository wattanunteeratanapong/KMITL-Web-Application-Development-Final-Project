using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Togeta.Models
{
    public class EventUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign key for User
        public User User { get; set; }  // Navigation property

        public int EventId { get; set; } // Foreign key for Event
        public Event Event { get; set; } // Navigation property
    }
}
