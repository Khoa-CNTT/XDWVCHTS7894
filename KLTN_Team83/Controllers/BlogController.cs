using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
