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
            _logger.LogInformation("Login attempt for user: {Username}", username);  // Log the login attempt

            // Authenticate the user
            var user = await _userService.LoginUserAsync(username, password);
            if (user != null)
            {
                _logger.LogInformation("User {Username} logged in successfully", username);  // Log successful login

                // Set session data
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Email", user.Email);

                // Log session data
                _logger.LogInformation($"Session Username: {HttpContext.Session.GetString("Username")}");
                _logger.LogInformation($"Session Email: {HttpContext.Session.GetString("Email")}");

                // Set cookie if "Remember Me" is checked
                if (rememberMe)
                {
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)  // Cookie expires in 30 days
                    };
                    Response.Cookies.Append("Username", user.Username, options);
                    _logger.LogInformation("Cookie set for user: {Username}", username);  // Log cookie set
                }

                return RedirectToAction("Index", "Home");  // Redirect to Home page after successful login
            }

            // If login fails
            _logger.LogWarning("Login failed for user: {Username}", username);  // Log failed login attempt
            ModelState.AddModelError("", "Invalid username or password");
            ViewData["LoginError"] = true; // Flag for error
            return View("Index");
        }
    }
}
