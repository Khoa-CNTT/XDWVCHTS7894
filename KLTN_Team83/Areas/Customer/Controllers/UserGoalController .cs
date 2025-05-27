using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize] // Người dùng phải đăng nhập để quản lý mục tiêu của họ
    public class UserGoalController : Controller // Hoặc bạn có thể đặt tên là GoalController nếu không bị trùng
    {
        // Action này sẽ trả về View HTML cho trang quản lý mục tiêu
        // URL có thể là /Customer/UserGoal/Index hoặc /Customer/Goal (nếu bạn đặt tên controller là GoalController)
        //public IActionResult Index()
        //{
        //    // Không cần lấy dữ liệu ở đây, JavaScript sẽ gọi API để lấy
        //    return View(); // Sẽ tìm View tại Areas/Customer/Views/UserGoal/Index.cshtml
        //}

        // Nếu bạn muốn truyền UserId xuống View để JavaScript sử dụng (ví dụ)
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.CurrentUserId = userId; // Hoặc truyền qua một ViewModel đơn giản
            return View();
        }
    }
}
