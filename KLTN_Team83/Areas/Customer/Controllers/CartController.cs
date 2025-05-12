using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Area("Customer")] // Đánh dấu controller này thuộc area Customer
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
