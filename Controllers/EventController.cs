using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Togeta.Data;
using Togeta.Models;

namespace Togeta.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inject both the ApplicationDBContext and IWebHostEnvironment
        public EventController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            IEnumerable<Event> allEvent = _db.Events;
            return View(allEvent);
        }
        public IActionResult DetailEachBox(int id)
        {
            var eventDetail = _db.Events.FirstOrDefault(e => e.Id == id);

            if (eventDetail == null)
            {
                return NotFound(); // Return 404 if no event is found
            }

            return View(eventDetail);
        }

        // Single Create action that handles both model saving and file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            // File upload handling
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Get file name and create the uploads folder path
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Build the complete file path
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // Store the relative path in the model (adjust folder name if needed)
                model.Imagepath = "/uploads/" + fileName;
            }
            else
            {
                // Handle the case where no file is uploaded.
                ModelState.AddModelError("ImageFile", "Please upload an image.");
                return View(model);
            }

            // Save the Event object to the database
            _db.Events.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Detail");
        }
    }
}
