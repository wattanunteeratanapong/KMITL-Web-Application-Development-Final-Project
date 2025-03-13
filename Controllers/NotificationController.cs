using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Togeta.Data;
using Togeta.Models;
using Microsoft.EntityFrameworkCore;

namespace Togeta.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDBContext _context;

        public NotificationController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Notification
        [HttpGet]
        public IActionResult GetNotifications()
        {
            // Get the username from session
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Get the user from the database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Unauthorized();
            }

            // Query notifications
            var notifications = _context.Notifications
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Message,
                    n.CreatedAt,
                    n.IsRead
                })
                .ToList();

            return Ok(notifications);
        }

        // POST: api/Notification/MarkAsRead
        [HttpPost("MarkAsRead")]
        public IActionResult MarkNotificationsAsRead()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Unauthorized();
            }

            var notifications = _context.Notifications
                .Where(n => n.UserId == user.Id && !n.IsRead)
                .ToList();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            _context.SaveChanges();

            return Ok(new { success = true, count = notifications.Count });
        }

        // POST: api/Notification/Clear
        [HttpPost("Clear")]
        public IActionResult ClearNotifications()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Unauthorized();
            }

            var notifications = _context.Notifications
                .Where(n => n.UserId == user.Id)
                .ToList();

            _context.Notifications.RemoveRange(notifications);
            _context.SaveChanges();

            return Ok(new { success = true, count = notifications.Count });
        }
    }
}