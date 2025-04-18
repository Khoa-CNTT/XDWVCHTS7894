using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Account> objAccountList = await _db.Accounts.ToListAsync();
            return View(objAccountList);
        }
        // CHỨC NĂNG THÊM ACCOUNT
        public IActionResult Create()
        {
            return View();
        }
        //Thêm account vào database
        [HttpPost]
        public IActionResult Create(Account obj)
        {
            ////kiểm tra có giống nhau hay ko
            if (obj.email == obj.passWord)
            {
                ModelState.AddModelError("passWord", "Mật khẩu không được trùng với tên đăng nhập.");
            }
            //obj.ngayTao = DateTime.Now;

            
            if (ModelState.IsValid)
            {
                _db.Accounts.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Account created successfully";
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        // CHỨC NĂNG SỬA ACCOUNT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Account? accFromDb = _db.Accounts.Find(id);
            if (accFromDb == null)
            {
                return NotFound();
            }
            return View(accFromDb);
        }
        //Thêm blog vào database
        [HttpPost]
        public IActionResult Edit(Account obj)
        {
            if (ModelState.IsValid)
            {
                _db.Accounts.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Account update successfully";
                return RedirectToAction("Index", "Account");
            }
            return View();
        }


        // CHỨC NĂNG XÓA ACCOUNT
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Account? accFromDb = _db.Accounts.Find(id);
            if (accFromDb == null)
            {
                return NotFound();
            }
            return View(accFromDb);
        }
        //Thêm blog vào database
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Account? obj = _db.Accounts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Accounts.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Account");

        }
    }
}
