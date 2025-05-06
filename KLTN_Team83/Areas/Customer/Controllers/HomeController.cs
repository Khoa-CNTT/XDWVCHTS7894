using System.Diagnostics;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Area("Customer")] // Đánh dấu controller này thuộc area Customer
    public class HomeController : Controller
    {
        // Khai báo logger để ghi log
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blog()
        {
            IEnumerable<Blog> BlogList = _unitOfWork.Blog.GetAll(includeProperties: "TypeBlog");
            return View(BlogList);
        }
        public IActionResult Plan()
        {
            return View();
        }
        public IActionResult Services()
        {
            IEnumerable<Product> ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(ProductList);
        }

        public IActionResult DetailProduct(int id) {
            Product Product = _unitOfWork.Product.Get(u=>u.Id_Product==id,includeProperties: "Category");
            return View(Product);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
