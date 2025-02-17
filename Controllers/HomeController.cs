using Microsoft.AspNetCore.Mvc;
using Togeta.Data;
using Togeta.Models;

namespace Togeta.Controllers
{
    public class HomeController : Controller
    {
        // Action for the default home page (Index)

        private readonly ApplicationDBContext _db;


        // Inject both the ApplicationDBContext and IWebHostEnvironment
        public HomeController(ApplicationDBContext db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<Event> allEvent = _db.Events;
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // If the session is null, redirect to the login page
                return RedirectToAction("Index", "Login");
            }

            ViewData["Username"] = username;  // Pass the session username to the view
            return View(allEvent);
        }
    }
}
