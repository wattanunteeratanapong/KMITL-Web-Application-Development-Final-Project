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
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(user);
                if (result)
                {
                    return RedirectToAction("Index", "Login"); 
                }
            }
            return View("Index");
        }
    }
}
