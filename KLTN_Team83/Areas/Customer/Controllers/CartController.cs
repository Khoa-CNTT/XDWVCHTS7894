using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
