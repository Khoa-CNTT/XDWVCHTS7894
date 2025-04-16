using KLTN_Team83.Data;
using Microsoft.AspNetCore.Mvc;
using KLTN_Team83.Models;

namespace KLTN_Team83.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await imageFile.CopyToAsync(ms);
                var imageData = ms.ToArray();

                var image = new MyImage
                {
                    FileName = imageFile.FileName,
                    ContentType = imageFile.ContentType,
                    ImageData = imageData
                };

                _context.MyImages.Add(image);
                await _context.SaveChangesAsync();

                return RedirectToAction("Display", new { id = image.Id });
            }

            return View("Error");
        }

        public IActionResult Display(int id)
        {
            var image = _context.MyImages.Find(id);
            if (image == null) return NotFound();

            return File(image.ImageData, image.ContentType);
        }
    }
}
