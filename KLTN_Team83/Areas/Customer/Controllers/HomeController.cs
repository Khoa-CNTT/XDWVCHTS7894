using System.Diagnostics;
using System.Security.Claims;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Authorization;
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
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id_Product == id, includeProperties: "Category"),
                Count = 1,
                Id_Product = id
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DetailProduct(ShoppingCart shoppingCart)
        {
            // Lấy thông tin người dùng từ ClaimsIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.Id_Product == shoppingCart.Id_Product);

            if (cartFromDb != null)
            {
                // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại trong giỏ hàng, thêm mới
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";

            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Services));
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
