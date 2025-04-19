using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Account> objAccountList = _unitOfWork.Account.GetAll().ToList();
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
            if (ModelState.IsValid)
            {
                _unitOfWork.Account.Add(obj);
                _unitOfWork.Save();
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
            Account? accFromDb = _unitOfWork.Account.Get(u=>u.id_Acc==id);
            if (accFromDb == null)
            {
                return NotFound();
            }
            return View(accFromDb);
        }
        //Sửa Account vào database
        [HttpPost]
        public IActionResult Edit(Account obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Account.Update(obj);
                _unitOfWork.Save();
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
            Account? accFromDb = _unitOfWork.Account.Get(u => u.id_Acc == id);
            if (accFromDb == null)
            {
                return NotFound();
            }
            return View(accFromDb);
        }
        //Xóa account khỏi database
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Account? obj = _unitOfWork.Account.Get(u => u.id_Acc == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Account.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Account");

        }
    }
}
