using System.Diagnostics;
using System.Security.Claims;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using KLTN_Team83.Models.ViewModels;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
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

        public IActionResult Goal()
        {
            return View();
        }

        public IActionResult Blog()
        {
            IEnumerable<Blog> BlogList = _unitOfWork.Blog.GetAll(includeProperties: "TypeBlog");
            return View(BlogList);
        }

        // --- ACTION METHOD CHO TRANG PLAN (GET) ---
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Plan()
        {
            var viewModel = new PlanVM();
            return View(viewModel);
        }

        // --- ACTION METHOD CHO TRANG PLAN (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Plan(PlanVM model)
        {
            model.IsSubmitted = true;

            if (ModelState.IsValid)
            {
                if (model.Weight.HasValue && model.Height.HasValue && model.Height.Value > 0)
                {
                    double heightInMeters = model.Height.Value / 100.0;
                    model.BMI = Math.Round(model.Weight.Value / (heightInMeters * heightInMeters), 2);
                    model.BmiCategory = GetBmiCategory(model.BMI.Value);

                    // Populate suggestions
                    model.NutritionalSuggestions = await GetNutritionalSuggestionsAsync(model);
                    model.ExerciseSuggestions = await GetExerciseSuggestionsAsync(model);

                    // Debug: Log the suggestions to verify they are set
                    _logger.LogInformation("Nutritional Suggestions Count: {Count}", model.NutritionalSuggestions.Count);
                    _logger.LogInformation("Exercise Suggestions Count: {Count}", model.ExerciseSuggestions.Count);
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng nhập cân nặng và chiều cao hợp lệ.");
                }
            }
            else
            {
                // Log ModelState errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("ModelState Error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(model);
        }

        // --- HELPER METHODS ---
        private string GetBmiCategory(double bmi)
        {
            if (bmi < 18.5) return "Gầy (Underweight)";
            if (bmi < 24.9) return "Bình thường (Normal weight)";
            if (bmi < 29.9) return "Thừa cân (Overweight)";
            return "Béo phì (Obesity)";
        }

        private async Task<List<string>> GetNutritionalSuggestionsAsync(PlanVM userInput)
        {
            var suggestions = new List<string>();
            if (userInput.BMI.HasValue)
            {
                suggestions.Add($"Dựa trên giới tính ({userInput.Gender}) và chỉ số BMI ({userInput.BMI:F2} - {userInput.BmiCategory}):");
                suggestions.Add("Luôn uống đủ nước (khoảng 2-2.5 lít/ngày tùy hoạt động).");
                suggestions.Add("Tăng cường rau xanh, trái cây trong mỗi bữa ăn.");

                if (userInput.BmiCategory.Contains("Thừa cân") || userInput.BmiCategory.Contains("Béo phì"))
                {
                    suggestions.Add("Hạn chế tối đa đồ ăn nhanh, thực phẩm chế biến sẵn, đồ ngọt và nước ngọt có ga.");
                    suggestions.Add("Kiểm soát khẩu phần ăn, không ăn quá no.");
                }
                else if (userInput.BmiCategory.Contains("Gầy"))
                {
                    suggestions.Add("Chia nhỏ bữa ăn thành 5-6 bữa/ngày để dễ hấp thu và không bỏ bữa.");
                    suggestions.Add("Đảm bảo đủ protein từ thịt, cá, trứng, sữa, các loại đậu, hạt để xây dựng cơ bắp.");
                }
                else
                {
                    suggestions.Add("Duy trì một chế độ ăn cân bằng, đa dạng các nhóm thực phẩm.");
                }
                suggestions.Add("Đây là những gợi ý chung, bạn nên tham khảo ý kiến chuyên gia dinh dưỡng để có kế hoạch phù hợp nhất.");
            }
            else
            {
                suggestions.Add("Vui lòng nhập đầy đủ thông tin cân nặng và chiều cao để nhận gợi ý.");
            }
            return await Task.FromResult(suggestions);
        }

        private async Task<List<string>> GetExerciseSuggestionsAsync(PlanVM userInput)
        {
            var suggestions = new List<string>();
            if (userInput.BMI.HasValue)
            {
                suggestions.Add($"Gợi ý vận động cho giới tính ({userInput.Gender}) và thể trạng ({userInput.BmiCategory}):");
                suggestions.Add("Duy trì vận động thể chất đều đặn ít nhất 150 phút/tuần với cường độ vừa phải.");
                if (userInput.BmiCategory.Contains("Thừa cân") || userInput.BmiCategory.Contains("Béo phì"))
                {
                    suggestions.Add("Ưu tiên các bài tập cardio đốt mỡ như đi bộ nhanh, chạy bộ, đạp xe, bơi lội.");
                }
                else if (userInput.BmiCategory.Contains("Gầy"))
                {
                    suggestions.Add("Tập trung vào các bài tập kháng lực để tăng cường sức mạnh và khối lượng cơ.");
                }
                suggestions.Add("Nếu có vấn đề sức khỏe, hãy tham khảo ý kiến bác sĩ trước khi bắt đầu một chương trình tập luyện mới.");
            }
            return await Task.FromResult(suggestions);
        }

        public IActionResult Services()
        {
            IEnumerable<Product> ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
            return View(ProductList);
        }

        public IActionResult DetailProduct(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id_Product == id, includeProperties: "Category,ProductImages"),
                Count = 1,
                Id_Product = id
            };
            if (cart.Product == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DetailProduct(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.Id_Product == shoppingCart.Id_Product);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";

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