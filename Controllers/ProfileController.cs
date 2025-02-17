using Microsoft.AspNetCore.Mvc;

namespace Togeta.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
