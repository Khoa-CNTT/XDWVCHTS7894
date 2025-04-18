using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        // CHỨC NĂNG ĐĂNG KÝ TÀI KHOẢN
        public IActionResult Register()
        {
            return View();
        }
        //Đăng ký tài khoản vào database
        [HttpPost]
        public IActionResult Register(Account obj)
        {
            ////kiểm tra có giống nhau hay ko
            if (obj.email == obj.passWord)
            {
                ModelState.AddModelError("email", "Mật khẩu không được trùng với Email.");
            }

            if (ModelState.IsValid)
            {
                _db.Accounts.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Account created successfully";
                return RedirectToAction("Index", "Blog");
            }
            return View();
        }
    }
}
