using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Togeta.Data;
using Togeta.Models;
using System.Linq;
using System.IO;

namespace Togeta.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProfileController(ApplicationDBContext context)
        {
            _context = context;
        }

        // ✅ แสดงหน้า Profile
        public IActionResult Index(int id)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null) return NotFound();

            ViewData["HideFooter"] = true; // ✅ ซ่อน Footer ในหน้านี้
            return View(profile);
        }

        // ✅ แสดงหน้าแก้ไขโปรไฟล์
        public IActionResult Edit(int id)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null) return NotFound();

            return View(profile);
        }

        // ✅ อัปเดตข้อมูลโปรไฟล์และอัปโหลดรูปภาพ
        [HttpPost]
        public IActionResult Edit(int id, Profile profile, IFormFile profilePicture, IFormFile coverPhoto)
        {
            var existingProfile = _context.Profiles.FirstOrDefault(p => p.Id == id);
            if (existingProfile == null) return NotFound();

            // ✅ อัปเดตข้อมูลโปรไฟล์
            existingProfile.FirstName = profile.FirstName;
            existingProfile.LastName = profile.LastName;
            existingProfile.Email = profile.Email;
            existingProfile.Bio = profile.Bio;
            existingProfile.Interests = profile.Interests;

            // ✅ อัปโหลดรูปภาพ (Profile Picture + Cover Photo)
            UploadProfileImages(existingProfile, profilePicture, coverPhoto);

            _context.SaveChanges();
            return RedirectToAction("Index", new { id = existingProfile.Id });
        }

        // ✅ ฟังก์ชันแยกอัปโหลดรูปภาพ เพื่อให้โค้ดอ่านง่ายขึ้น
        private void UploadProfileImages(Profile profile, IFormFile profilePicture, IFormFile coverPhoto)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // ✅ อัปโหลด Profile Picture
            if (profilePicture != null)
            {
                var profilePicPath = Path.Combine(uploadPath, profilePicture.FileName);
                using (var stream = new FileStream(profilePicPath, FileMode.Create))
                {
                    profilePicture.CopyTo(stream);
                }
                profile.ProfilePictureUrl = "/uploads/" + profilePicture.FileName;
            }

            // ✅ อัปโหลด Cover Photo
            if (coverPhoto != null)
            {
                var coverPicPath = Path.Combine(uploadPath, coverPhoto.FileName);
                using (var stream = new FileStream(coverPicPath, FileMode.Create))
                {
                    coverPhoto.CopyTo(stream);
                }
                profile.CoverPhotoUrl = "/uploads/" + coverPhoto.FileName;
            }
        }
    }
}
