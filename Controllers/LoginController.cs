using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Togeta.Models;
using Togeta.Services;

namespace Togeta.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService userService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe = false)
        {
            _logger.LogInformation("Login attempt for user: {Username}", username);  

            var user = await _userService.LoginUserAsync(username, password);
            if (user != null)
            {
                _logger.LogInformation("User {Username} logged in successfully", username);  

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Email", user.Email);

                _logger.LogInformation($"Session Username: {HttpContext.Session.GetString("Username")}");
                _logger.LogInformation($"Session Email: {HttpContext.Session.GetString("Email")}");

                if (rememberMe)
                {
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)  
                    };
                    Response.Cookies.Append("Username", user.Username, options);
                    _logger.LogInformation("Cookie set for user: {Username}", username);  
                }

                return RedirectToAction("Index", "Home");  
            }

            // If login fails
            _logger.LogWarning("Login failed for user: {Username}", username);  // Log failed login attempt
            ModelState.AddModelError("", "Invalid username or password");
            ViewData["LoginError"] = true; // Flag for error
            return View("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToAction("Index", "Login"); // or wherever your login page is
        }

    }
}
