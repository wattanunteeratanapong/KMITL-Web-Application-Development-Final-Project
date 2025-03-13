using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Togeta.Data;
using Togeta.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Togeta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _db;

        public HomeController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? category = null, string? search = null)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            ViewData["ProfileImage"] = user?.ProfileImagePath ?? "profile icon.png";
            ViewData["Username"] = username;

            // Start building the query
            var query = _db.Events.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Tag == category);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search)
                                         || e.Detail.Contains(search)
                                         || e.location.Contains(search));
            }

            var now = DateTime.Now;
            query = query.Where(e => e.ClosedDate == null || e.ClosedDate > now);
            query = query.OrderBy(e => e.StartDateTime);

            var events = query.Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                location = e.location,
                Participants = e.Participants,
                Tag = e.Tag,
                Detail = e.Detail,
                Imagepath = e.Imagepath.StartsWith("/uploads/") ? e.Imagepath : "/uploads/" + e.Imagepath,
                ImageFile = null,
                CreatorId = e.CreatorId,
                ClosedDate = e.ClosedDate
            }).ToList();

            // For participant counts in Index.cshtml (for Razor rendering)
            var eventUsers = _db.EventUsers.ToList();
            ViewBag.EventUsers = eventUsers;

            return View(events);
        }

        public IActionResult CalendarView()
        {
            var allEvents = _db.Events
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.StartDateTime,
                    e.EndDateTime
                })
                .ToList();

            var eventsByDate = allEvents
                .GroupBy(e => e.StartDateTime.Date)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => g.Select(ev => ev).ToList()
                );

            ViewBag.EventsByDate = eventsByDate;
            return View();
        }

        [HttpGet]
        public IActionResult SearchEvents(string query, string tag)
        {
            // 1) Start with all events
            var results = _db.Events.AsQueryable();

            // 2) Exclude closed events (those with ClosedDate <= now)
            results = results.Where(e => e.ClosedDate == null || e.ClosedDate > DateTime.Now);

            // 3) Filter by tag if needed
            if (!string.IsNullOrEmpty(tag))
            {
                results = results.Where(e => e.Tag == tag);
            }

            // 4) Filter by query if needed
            if (!string.IsNullOrEmpty(query))
            {
                results = results.Where(e => e.Name.Contains(query)
                    || e.Detail.Contains(query)
                    || e.location.Contains(query));
            }

            // 5) Order by StartDateTime
            results = results.OrderBy(e => e.StartDateTime);

            // 6) Select needed fields (including joinedCount for display)
            var list = results
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Tag,
                    e.Participants,
                    joinedCount = _db.EventUsers.Count(eu => eu.EventId == e.Id),
                    e.StartDateTime,
                    e.EndDateTime,
                    e.Detail,
                    imagepath = e.Imagepath.StartsWith("/uploads/")
                        ? e.Imagepath
                        : "/uploads/" + e.Imagepath
                })
                .ToList();

            // 7) Return as JSON
            return Json(new { events = list });
        }


        [HttpGet]
        public IActionResult GetEventsByDate(string date, string tag)
        {
            if (!DateTime.TryParse(date, out DateTime selectedDate))
            {
                return BadRequest("Invalid date format.");
            }

            // Filter events whose StartDateTime is on that date and are not closed yet
            var query = _db.Events
                .Where(e => e.StartDateTime.Date <= selectedDate.Date &&
            e.EndDateTime.Date >= selectedDate.Date &&
            e.EndDateTime > DateTime.Now &&
            (e.ClosedDate == null || e.ClosedDate > DateTime.Now))
                .AsQueryable();

            // If tag is not empty, filter by that tag
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(e => e.Tag == tag);
            }

            // Return JSON with joinedCount
            var matchedEvents = query
                .Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    startDateTime = e.StartDateTime,
                    endDateTime = e.EndDateTime,
                    location = e.location,
                    participants = e.Participants,
                    tag = e.Tag,
                    detail = e.Detail,
                    imagepath = e.Imagepath.StartsWith("/uploads/")
                        ? e.Imagepath
                        : "/uploads/" + e.Imagepath,
                    joinedCount = _db.EventUsers.Count(eu => eu.EventId == e.Id)
                })
                .ToList();

            return Json(matchedEvents);
        }

        [HttpGet]
        public IActionResult GetUserEventsByDate(string date)
        {
            if (!DateTime.TryParse(date, out DateTime selectedDate))
            {
                return BadRequest("Invalid date format.");
            }

            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Json(Enumerable.Empty<object>());
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Json(Enumerable.Empty<object>());
            }

            var joinedEventIds = _db.EventUsers
                .Where(eu => eu.UserId == user.Id)
                .Select(eu => eu.EventId)
                .ToList();

            var matchedEvents = _db.Events
                .Where(e => joinedEventIds.Contains(e.Id) &&
     e.StartDateTime.Date <= selectedDate.Date &&
     e.EndDateTime.Date >= selectedDate.Date &&
     e.EndDateTime > DateTime.Now &&
     (e.ClosedDate == null || e.ClosedDate > DateTime.Now)
)
                .Select(e => new
                {
                    e.Id,
                    e.Tag,
                    e.Name,
                    e.StartDateTime,
                    e.EndDateTime,
                    e.Detail
                })
                .ToList();

            return Json(matchedEvents);
        }

        [HttpGet]
        public IActionResult GetSpecialEvents()
        {
            // 1) Identify current user
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { recommended = new List<object>(), expiring = new List<object>(), closing = new List<object>() });
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Json(new { recommended = new List<object>(), expiring = new List<object>(), closing = new List<object>() });
            }

            // 2) Find user’s joined events -> extract tags
            var userEventIds = _db.EventUsers
                .Where(eu => eu.UserId == user.Id)
                .Select(eu => eu.EventId)
                .ToList();

            var userJoinedTags = _db.Events
                .Where(e => userEventIds.Contains(e.Id))
                .Select(e => e.Tag)
                .Distinct()
                .ToList();

            // 3) Recommended: events matching the user’s favorite tags, but not yet joined
            //    (and not closed yet)
            var recommendedQuery = _db.Events
                .Where(e => userJoinedTags.Contains(e.Tag)
                            && !userEventIds.Contains(e.Id)
                            && (e.ClosedDate == null || e.ClosedDate > DateTime.Now));

            var recommended = recommendedQuery
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Tag,
                    e.Participants,
                    JoinedCount = _db.EventUsers.Count(eu => eu.EventId == e.Id),
                    e.StartDateTime,
                    e.EndDateTime
                })
                .OrderBy(e => e.StartDateTime)
                .Take(5)  // limit how many recommended events to show
                .ToList();

            // 4) Expiring Soon: near participant limit (e.g., within 2 spots left)
            //    Also not closed yet
            int threshold = 2; // or any other definition of "expiring soon"
            var expiringQuery = _db.Events
                .Where(e => (e.ClosedDate == null || e.ClosedDate > DateTime.Now))
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Tag,
                    e.Participants,
                    JoinedCount = _db.EventUsers.Count(eu => eu.EventId == e.Id),
                    e.StartDateTime,
                    e.EndDateTime
                })
                .AsEnumerable()
                .Where(e => (e.Participants - e.JoinedCount) <= threshold && (e.Participants - e.JoinedCount) > 0)
                .OrderBy(e => (e.Participants - e.JoinedCount)) // soonest to fill up
                .Take(5)
                .ToList();

            // 5) Closing Soon: events whose ClosedDate is within the next X days/hours
            //    (and not closed yet)
            DateTime soonThreshold = DateTime.Now.AddDays(2); // e.g. closing within 2 days
            var closingQuery = _db.Events
                .Where(e => e.ClosedDate != null
                            && e.ClosedDate > DateTime.Now
                            && e.ClosedDate < soonThreshold)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Tag,
                    e.Participants,
                    JoinedCount = _db.EventUsers.Count(eu => eu.EventId == e.Id),
                    e.StartDateTime,
                    e.EndDateTime,
                    e.ClosedDate
                })
                .OrderBy(e => e.ClosedDate)
                .Take(5)
                .ToList();

            return Json(new
            {
                recommended,
                expiring = expiringQuery,
                closing = closingQuery
            });
        }

    }
}
