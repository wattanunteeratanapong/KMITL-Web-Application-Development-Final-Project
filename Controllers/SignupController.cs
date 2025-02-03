using Microsoft.AspNetCore.Mvc;

namespace BasicASP.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
