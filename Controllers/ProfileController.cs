using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Togeta.Data;
using Togeta.Models;
using Microsoft.EntityFrameworkCore;

namespace Togeta.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProfileController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            // 1) เช็กว่า user ล็อกอินหรือยัง
            var usernameInSession = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(usernameInSession))
            {
                return RedirectToAction("Index", "Login");
            }

            // 2) ดึง user ปัจจุบัน (สำหรับ Navbar)
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == usernameInSession);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // 3) ตั้งค่าโปรไฟล์ใน Navbar ให้ใช้รูปของ currentUser เท่านั้น
            ViewData["ProfileImage"] = Url.Content(currentUser.ProfileImagePath ?? "/default-profile.jpg");

            // 4) ถ้าไม่มี id => แสดงโปรไฟล์ตัวเอง / ถ้ามี id => แสดงโปรไฟล์ user นั้น
            User userToDisplay;
            if (!id.HasValue)
            {
                // แสดงโปรไฟล์ของตัวเอง
                userToDisplay = currentUser;
            }
            else
            {
                // แสดงโปรไฟล์ user คนอื่นตาม id
                userToDisplay = _context.Users.FirstOrDefault(u => u.Id == id.Value);
                if (userToDisplay == null)
                {
                    return NotFound("User not found.");
                }
            }

            // 5) กิจกรรมที่ userToDisplay เป็นโฮสต์
            var eventsHosted = _context.Events
                .Where(e => e.CreatorId == userToDisplay.Id)
                .ToList();

            // 6) กิจกรรมที่ userToDisplay เข้าร่วม (EventUsers)
            //    => But exclude the events they host themselves.
            var allEventUsers = _context.EventUsers
                .Where(eu => eu.UserId == userToDisplay.Id)
                .Include(eu => eu.Event)
                .ToList();

            // Filter out any event where userToDisplay is also the host
            var eventUsersJoinedNotHosted = allEventUsers
                .Where(eu => eu.Event.CreatorId != userToDisplay.Id)
                .ToList();

            // ดึงรีวิว (comments) ที่ userToDisplay เป็นผู้รับ
            var reviews = _context.Comments
                .Where(c => c.ReceiverId == userToDisplay.Id)
                .Include(c => c.Event)
                .ToList();

            // โหลดรายชื่อผู้ใช้ทั้งหมด (ถ้าจำเป็น)
            var users = _context.Users.ToList();
            ViewBag.Users = users;

            // Pass the data to the view
            ViewBag.Events = eventsHosted;                // Hosted events
            ViewBag.EventUsers = eventUsersJoinedNotHosted; // Joined events (excluding those the user hosts)
            ViewBag.User = userToDisplay; // ใครก็ตามที่กำลังดูโปรไฟล์
            ViewBag.Comment = reviews;
            ViewBag.Gender = currentUser.Gender;

            return View(userToDisplay);
        }

        public IActionResult Edit()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // ตั้งค่าโปรไฟล์ใน Navbar
            ViewData["ProfileImage"] = Url.Content(user.ProfileImagePath ?? "/default-profile.jpg");
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateProfile(User userModel, IFormFile ProfileImage, IFormFile CoverImage, string[] SelectedInterests)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userModel.Id);
            if (user != null)
            {
                // อัปเดต Bio
                user.Bio = userModel.Bio;

                // อัปเดต Interests
                if (SelectedInterests != null && SelectedInterests.Any())
                {
                    user.Interests = string.Join(", ", SelectedInterests);
                }
                else
                {
                    user.Interests = string.Empty;
                }

                // อัปโหลดรูปโปรไฟล์
                if (ProfileImage != null)
                {
                    var profilePath = Path.Combine("wwwroot/uploads", ProfileImage.FileName);
                    using (var stream = new FileStream(profilePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(stream);
                    }
                    user.ProfileImagePath = "/uploads/" + ProfileImage.FileName;
                }
                else if (string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    // ยังไม่มีรูปโปรไฟล์ => ให้ default
                    user.ProfileImagePath = "/default-profile.jpg";
                }

                // อัปโหลดรูปหน้าปก
                if (CoverImage != null)
                {
                    var coverPath = Path.Combine("wwwroot/uploads", CoverImage.FileName);
                    using (var stream = new FileStream(coverPath, FileMode.Create))
                    {
                        CoverImage.CopyTo(stream);
                    }
                    user.CoverImagePath = "/uploads/" + CoverImage.FileName;
                }
                else if (string.IsNullOrEmpty(user.CoverImagePath))
                {
                    // ยังไม่มีรูป Cover => ให้ default
                    user.CoverImagePath = "/default-cover.jpg";
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
