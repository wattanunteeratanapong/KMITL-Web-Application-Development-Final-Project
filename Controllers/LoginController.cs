using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Togeta.Data; // ✅ เพิ่มเพื่อเข้าถึงฐานข้อมูล
using Togeta.Models;

namespace Togeta.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ApplicationDBContext _context; // ✅ เพิ่ม Database Context

        public LoginController(ILogger<LoginController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context; // ✅ ใช้ฐานข้อมูล
        }

        public IActionResult Index()
        {
            return View();
            //  dklsjflksadjflkj
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (existingUser != null)
            {
                // ✅ ถ้าล็อกอินสำเร็จ ให้พาไปที่หน้าโปรไฟล์
                return RedirectToAction("Index", "Profile", new { id = existingUser.Id });
            }

            ViewBag.Error = "Invalid username or password";
            return View("Index");
        }
    }
}
