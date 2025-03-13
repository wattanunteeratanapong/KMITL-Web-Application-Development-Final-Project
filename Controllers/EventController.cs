using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Togeta.Data;
using Togeta.Models;
using Microsoft.EntityFrameworkCore;
using Togeta.Services; // Make sure this namespace is correct for EmailService

namespace Togeta.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService _emailService;

        public EventController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment, EmailService emailService)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            // Set the profile icon path for the navbar
            ViewData["ProfileImage"] = user?.ProfileImagePath ?? "~/profile icon.png";

            return View();
        }


        public IActionResult Detail()
        {
            var allEvents = _db.Events.ToList();
            return View(allEvents);
        }

        public IActionResult DetailEachBox(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            // Set the profile icon and current user id for the navbar
            ViewData["ProfileImage"] = user?.ProfileImagePath ?? "~/profile icon.png";
            ViewBag.CurrentUserId = user?.Id;  // <-- Add this line

            var eventDetails = _db.Events.FirstOrDefault(e => e.Id == id);
            if (eventDetails == null)
            {
                return NotFound();
            }

            // Set all comments for this event so the view can check if the user has commented
            ViewBag.AllComments = _db.Comments.Where(c => c.EventId == id).ToList();

            bool hasJoined = user != null && _db.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == id);
            ViewBag.HasJoined = hasJoined;

            var timeSpan = System.DateTime.Now - eventDetails.CreatedAt;
            ViewBag.CreateSince = $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")} " +
                                    $"{timeSpan.Hours} hr{(timeSpan.Hours > 1 ? "s" : "")} " +
                                    $"{timeSpan.Minutes} min{(timeSpan.Minutes > 1 ? "s" : "")}";

            int currentParticipants = _db.EventUsers.Count(eu => eu.EventId == id);
            ViewBag.CurrentParticipants = currentParticipants;

            var participants = _db.EventUsers
                .Where(eu => eu.EventId == id)
                .Join(_db.Users, eu => eu.UserId, u => u.Id, (eu, u) => u)
                .ToList();
            ViewBag.Participants = participants;

            bool isCreator = user != null && eventDetails.CreatorId == user.Id;
            ViewBag.IsCreator = isCreator;

            var hostUser = _db.Users.FirstOrDefault(u => u.Id == eventDetails.CreatorId);
            ViewBag.HostUser = hostUser;

            return View(eventDetails);
        }


        [HttpPost]
        public IActionResult KickParticipant(int eventId, int userId)
        {
            var currentUsername = HttpContext.Session.GetString("Username");
            var currentUser = _db.Users.FirstOrDefault(u => u.Username == currentUsername);
            var eventDetails = _db.Events.FirstOrDefault(e => e.Id == eventId);

            // If not logged in or not the host -> redirect
            if (string.IsNullOrEmpty(currentUsername) || currentUser == null)
                return RedirectToAction("Index", "Login");
            if (eventDetails == null)
                return NotFound();
            if (eventDetails.CreatorId != currentUser.Id)
                return RedirectToAction("DetailEachBox", new { id = eventId });

            // Kick the participant
            var eventUser = _db.EventUsers.FirstOrDefault(eu => eu.EventId == eventId && eu.UserId == userId);
            if (eventUser == null) return NotFound();

            _db.EventUsers.Remove(eventUser);
            _db.SaveChanges();

            // Create a notification record
            var notification = new Notification
            {
                EventId = eventId,
                UserId = userId,
                Message = $"You have been kicked from event: {eventDetails.Name}",
                IsRead = false,
                CreatedAt = System.DateTime.Now
            };

            _db.Notifications.Add(notification);
            _db.SaveChanges();

            // Get the kicked user's details to retrieve their email
            var kickedUser = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (kickedUser != null && !string.IsNullOrEmpty(kickedUser.Email))
            {
                string emailSubject = $"You have been removed from event: {eventDetails.Name}";
                string emailBody = $"Hello {kickedUser.FirstName},<br/><br/>" +
                                   $"You have been removed from the event \"{eventDetails.Name}\".<br/><br/>" +
                                   "If you have any questions, please contact the event host.<br/><br/>" +
                                   "Regards,<br/>Togeta Team";

                try
                {
                    _emailService.SendNotificationEmail(kickedUser.Email, emailSubject, emailBody);
                }
                catch (System.Exception ex)
                {
                    // Optionally log the error, but continue with the flow
                    // For example: _logger.LogError(ex, "Failed to send email notification.");
                }
            }

            return RedirectToAction("DetailEachBox", new { id = eventId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            ViewData["ProfileImage"] = user?.ProfileImagePath ?? "~/profile icon.png";

            if (user == null) return RedirectToAction("Index", "Login");

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                model.Imagepath = "/uploads/" + fileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please upload an image.");
                return View(model);
            }

            model.CreatorId = user.Id;
            model.CreatedAt = System.DateTime.Now;
            _db.Events.Add(model);
            await _db.SaveChangesAsync();

            // Join the creator to the event
            _db.EventUsers.Add(new EventUser { EventId = model.Id, UserId = user.Id });
            _db.SaveChanges();

            // Redirect to the event's DetailEachBox page, passing the newly created event’s ID
            return RedirectToAction("DetailEachBox", new { id = model.Id });
        }


        [HttpPost]
        public IActionResult Join(int eventId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var selectedEvent = _db.Events.FirstOrDefault(e => e.Id == eventId);
            if (selectedEvent == null)
            {
                return NotFound();
            }

            // 1) Check if event is closed
            if (selectedEvent.ClosedDate != null && selectedEvent.ClosedDate <= System.DateTime.Now)
            {
                TempData["ErrorMessage"] = "This event has already closed!";
                return RedirectToAction("DetailEachBox", new { id = eventId });
            }

            // 2) Check if event is full
            int currentParticipants = _db.EventUsers.Count(eu => eu.EventId == eventId);
            if (currentParticipants >= selectedEvent.Participants)
            {
                selectedEvent.ClosedDate = System.DateTime.Now;
                _db.SaveChanges();
                TempData["ErrorMessage"] = "The event is now full and has been closed!";
                return RedirectToAction("DetailEachBox", new { id = eventId });
            }

            // 3) Check if user already joined
            bool isUserAlreadyJoined = _db.EventUsers
                .Any(eu => eu.EventId == eventId && eu.UserId == user.Id);
            if (isUserAlreadyJoined)
            {
                return RedirectToAction("DetailEachBox", new { id = eventId });
            }

            // 4) Add user to the event
            var eventUser = new EventUser
            {
                EventId = eventId,
                UserId = user.Id
            };
            _db.EventUsers.Add(eventUser);
            _db.SaveChanges();

            // 5) If full after adding user, close the event
            currentParticipants = _db.EventUsers.Count(eu => eu.EventId == eventId);
            if (currentParticipants >= selectedEvent.Participants)
            {
                selectedEvent.ClosedDate = System.DateTime.Now;
                _db.SaveChanges();
            }

            // 6) Create a notification record for the event owner
            var notification = new Notification
            {
                EventId = eventId,
                UserId = selectedEvent.CreatorId, // Notify the event creator (owner)
                Message = $"{user.FirstName} has joined your event: {selectedEvent.Name}",
                IsRead = false,
                CreatedAt = System.DateTime.Now
            };
            _db.Notifications.Add(notification);
            _db.SaveChanges();

            // 7) Send an email notification to the event owner if they have an email address.
            var owner = _db.Users.FirstOrDefault(u => u.Id == selectedEvent.CreatorId);
            if (owner != null && !string.IsNullOrEmpty(owner.Email))
            {
                string emailSubject = $"New participant joined your event: {selectedEvent.Name}";
                string emailBody = $"Hello {owner.FirstName},<br/><br/>" +
                                $"{user.FirstName} has just joined your event \"{selectedEvent.Name}\".<br/><br/>" +
                                "Thank you for using Togeta!<br/><br/>" +
                                "Regards,<br/>Togeta Team";
                try
                {
                    _emailService.SendNotificationEmail(owner.Email, emailSubject, emailBody);
                }
                catch (System.Exception ex)
                {
                    // Optional: log the exception, for example:
                    // _logger.LogError(ex, "Failed to send join event email notification.");
                }
            }

            return RedirectToAction("DetailEachBox", new { id = eventId });
        }

        // ===========================
        // ADD: CloseEvent & CancelEvent
        // ===========================
        [HttpPost]
        public IActionResult CloseEvent(int eventId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var selectedEvent = _db.Events
                                .Include(e => e.EventUsers)
                                .FirstOrDefault(e => e.Id == eventId);
            if (selectedEvent == null)
            {
                return NotFound();
            }

            // Check if user is host
            if (selectedEvent.CreatorId != user.Id)
            {
                // Not the host => redirect back
                return RedirectToAction("DetailEachBox", new { id = eventId });
            }

            // Close the event
            selectedEvent.ClosedDate = System.DateTime.Now;
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Event has been closed!";

            // Get all participants except the host
            var participantIds = _db.EventUsers
                .Where(eu => eu.EventId == eventId && eu.UserId != selectedEvent.CreatorId)
                .Select(eu => eu.UserId)
                .ToList();

            var participants = _db.Users.Where(u => participantIds.Contains(u.Id)).ToList();

            // Create in-app notifications for each participant
            foreach (var participantId in participantIds)
            {
                var notification = new Notification
                {
                    EventId = eventId,
                    UserId = participantId,
                    Message = $"Event '{selectedEvent.Name}' has been closed by the host.",
                    IsRead = false,
                    CreatedAt = System.DateTime.Now
                };
                _db.Notifications.Add(notification);
            }
            _db.SaveChanges();

            // Send email notifications
            string emailSubject = $"Event Closed: {selectedEvent.Name}";
            string emailBody = $"Hello,<br/><br/>" +
                $"The event \"{selectedEvent.Name}\" has been closed by the host.<br/><br/>" +
                "Thank you for participating,<br/>Togeta Team";

            foreach (var participant in participants)
            {
                if (!string.IsNullOrEmpty(participant.Email))
                {
                    try
                    {
                        _emailService.SendNotificationEmail(participant.Email, emailSubject, emailBody);
                    }
                    catch (System.Exception ex)
                    {
                        // Optionally log the error here
                    }
                }
            }

            return RedirectToAction("DetailEachBox", new { id = eventId });
        }

        [HttpPost]
        public IActionResult CancelEvent(int eventId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var selectedEvent = _db.Events.FirstOrDefault(e => e.Id == eventId);
            if (selectedEvent == null)
            {
                return NotFound();
            }

            // Check if user is host
            if (selectedEvent.CreatorId != user.Id)
            {
                // Not the host => redirect back to event detail
                return RedirectToAction("DetailEachBox", new { id = eventId });
            }

            // Get event details for notifications before deletion
            string eventName = selectedEvent.Name;

            // Get all participants except the host
            var participantIds = _db.EventUsers
                .Where(eu => eu.EventId == eventId && eu.UserId != selectedEvent.CreatorId)
                .Select(eu => eu.UserId)
                .ToList();

            var participants = _db.Users.Where(u => participantIds.Contains(u.Id)).ToList();

            // UPDATE existing notifications to set EventId to null
            var existingNotifications = _db.Notifications.Where(n => n.EventId == eventId);
            foreach (var notification in existingNotifications)
            {
                notification.EventId = null;
            }
            _db.SaveChanges();

            // Create new "cancelled event" notifications
            foreach (var participantId in participantIds)
            {
                var notification = new Notification
                {
                    EventId = null, // Now possible with our updated schema
                    UserId = participantId,
                    Message = $"Event '{eventName}' has been cancelled by the host.",
                    IsRead = false,
                    CreatedAt = System.DateTime.Now
                };
                _db.Notifications.Add(notification);
            }

            // Save the new notifications
            _db.SaveChanges();

            // Prepare email details
            string emailSubject = $"Event Cancelled: {eventName}";
            string emailBody = $"Hello,<br/><br/>" +
                $"We regret to inform you that the event \"{eventName}\" has been cancelled by the host.<br/><br/>" +
                "We apologize for any inconvenience.<br/><br/>" +
                "Regards,<br/>Togeta Team";

            // Now remove event participants
            var eventUsers = _db.EventUsers.Where(eu => eu.EventId == eventId);
            _db.EventUsers.RemoveRange(eventUsers);

            // Remove the event
            _db.Events.Remove(selectedEvent);

            // Save these changes 
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Event has been canceled and removed.";

            // Send email notification to each participant
            foreach (var participant in participants)
            {
                if (!string.IsNullOrEmpty(participant.Email))
                {
                    try
                    {
                        _emailService.SendNotificationEmail(participant.Email, emailSubject, emailBody);
                    }
                    catch (System.Exception ex)
                    {
                        // Optionally log the error
                    }
                }
            }

            // Redirect to HOME page
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Comment(int eventId, int ReceiverId, string content, int creditscore)
        {
            var username = HttpContext.Session.GetString("Username");
            var currentUser = _db.Users.FirstOrDefault(u => u.Username == username);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // 1) Create & save new comment
            var newComment = new Comment
            {
                EventId = eventId,
                SenderId = currentUser.Id,
                ReceiverId = ReceiverId,
                Content = content,
                Creditscore = creditscore,
                CreatedAt = DateTime.Now
            };

            _db.Comments.Add(newComment);
            _db.SaveChanges();

            // 2) Update the receiver's average credit score
            var receiver = _db.Users.FirstOrDefault(u => u.Id == ReceiverId);
            if (receiver != null)
            {
                var userComments = _db.Comments
                    .Where(c => c.ReceiverId == ReceiverId)
                    .ToList();

                if (userComments.Any())
                {
                    double averageScore = userComments.Average(c => c.Creditscore);
                    receiver.CreditScore = (float)averageScore;

                    // 3) Update the receiver’s total comment count
                    //    If you want the total # of distinct senders, you could do .Select(c => c.SenderId).Distinct().Count()
                    //    but if you want the total # of comments, just do Count():
                    receiver.CommentCount = userComments.Count;

                    _db.SaveChanges();
                }
            }

            // 4) Redirect back to the Event detail page
            return RedirectToAction("DetailEachBox", new { id = eventId });
        }


    }
}
