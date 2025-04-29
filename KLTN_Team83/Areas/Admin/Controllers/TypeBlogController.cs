using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    
    public class TypeBlogController : Controller
    {
        private readonly IUnitOfWork _db;
        public TypeBlogController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<TypeBlog> objTypeBlogList = _db.TypeBlog.GetAll().ToList();
            return View(objTypeBlogList);
        }

        // CHỨC NĂNG THÊM TYPEBLOG
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TypeBlog obj)
        {
            if (ModelState.IsValid)
            {
                _db.TypeBlog.Add(obj);
                _db.Save();
                TempData["success"] = "TypeBlog created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // CHỨC NĂNG SỬA TYPEBLOG
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            TypeBlog? typeBlogFromDb = _db.TypeBlog.Get(u=>u.id_TypeBlog==id);
            if (typeBlogFromDb == null)
            {
                return NotFound();
            }
            return View(typeBlogFromDb);
        }
        [HttpPost]
        public IActionResult Edit(TypeBlog obj)
        {
            if (obj.Name == obj.Description.ToString())
            {
                ModelState.AddModelError("Name", "Description cannot be the same as Name.");
            }
            if (obj.Name == null || obj.Name.ToLower() == "")
            {
                ModelState.AddModelError("", "TypeBlog Name cannot be empty!");
            }
            if (_db.TypeBlog.GetAll().Any(u => u.Name == obj.Name))
            {
                ModelState.AddModelError("Name", "TypeBlog Name already exists!");
            }
            if (ModelState.IsValid)
            {
                _db.TypeBlog.Update(obj);
                _db.Save();
                TempData["success"] = "TypeBlog updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // CHỨC NĂNG XÓA TYPEBLOG
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            TypeBlog? typeBlogFromDb = _db.TypeBlog.Get(u => u.id_TypeBlog == id);
            if (typeBlogFromDb == null)
            {
                return NotFound();
            }
            return View(typeBlogFromDb);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            TypeBlog? obj = _db.TypeBlog.Get(u => u.id_TypeBlog == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.TypeBlog.Remove(obj);
            _db.Save();
            TempData["success"] = "TypeBlog deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
