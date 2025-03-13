using System;
using Togeta.Models; // Add this if Event and User are in Togeta.Models
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Togeta.Models
{
    public class Notification
    {
        public int Id { get; set; } // Primary Key
        public int? EventId { get; set; } // The event the notification is related to
        public int UserId { get; set; } // The user who will receive the notification
        public string Message { get; set; } // The notification message
        public bool IsRead { get; set; } = false; // Whether the notification has been read
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp of the notification

        // Navigation properties
        public Event Event { get; set; } // Add this if Event exists
        public User User { get; set; } // Add this if User exists
    }
}
