using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using KLTN_Team83.Models.ViewModels;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagment(string userId)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(u=>u.UserId==userId).RoleId;
            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u=>u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _db.Roles.Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id_Company.ToString()
                }),
            };
            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            if(!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagmentVM.ApplicationUser.Id);
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.Id_Company = roleManagmentVM.ApplicationUser.Id_Company;
                }
                if(oldRole == SD.Role_Company)
                {
                    applicationUser.Id_Company = null;
                }
                _db.SaveChanges();
                
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Upsert(string? id)
        {
            ApplicationUser objUser = new();
            if (id == null || id == "0")
            {
                //create
                return View(objUser);
            }
            else
            {
                //update
                objUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
                if (objUser == null)
                {
                    return NotFound();
                }
                return View(objUser);
            }
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u=>u.Company).ToList();

            var userRole = _db.UserRoles.ToList();
            var role = _db.Roles.ToList();

            foreach (var user in objUserList)
            {
                //Role
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = role.FirstOrDefault(u => u.Id == roleId).Name;

                //Company
                if (user.Company == null)
                {
                    user.Company = new()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = objUserList });
        }
        // CHỨC NĂNG XÓA USER
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string? id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // Đang khóa, cần mở khóa
                objFromDb.LockoutEnd = DateTime.Now;

                // Hoặc:
                //objFromDb.LockoutEnd = null;
            }
            else
            {
                // Chưa khóa, cần khóa
                objFromDb.LockoutEnd = DateTime.Now.AddYears(10);
            }
            _db.SaveChanges();

            return Json(new { success = true, message = "Lock/Unlock Successful" });
        }
        // CHỨC NĂNG XÓA USER
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
