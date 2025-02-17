using Microsoft.AspNetCore.Mvc;
using Togeta.Models;
using Togeta.Services;

namespace Togeta.Controllers
{
    public class SignupController : Controller
    {
        private readonly IUserService _userService;

        public SignupController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            // Check if the username already exists
            var existingUser = await _userService.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                // Set error message for duplicate username
                ViewData["UsernameError"] = "Username is already taken.";
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(user);
                if (result)
                {
                    // Set session after successful registration
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Email", user.Email);

                    return RedirectToAction("Index", "Login");  // Redirect to login page after successful registration
                }
            }

            return View("Index");
        }
    }
}
