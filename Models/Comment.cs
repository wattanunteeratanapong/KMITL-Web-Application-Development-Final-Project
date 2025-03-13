using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Togeta.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary Key

        public int EventId { get; set; }  // Foreign Key for Event
        public Event Event { get; set; }  // Navigation property

        public int SenderId { get; set; }  // Foreign Key for User (who wrote the comment)
        

        public int ReceiverId { get; set; }  // Optional (if replying to a specific user)
        

        public string? Content { get; set; }  // The actual comment

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Timestamp
        public int Creditscore { get; set; }
    }
}
