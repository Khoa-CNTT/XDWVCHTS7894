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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _repo;
        public CategoryController(IUnitOfWork db)
        {
            _repo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _repo.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        // CHỨC NĂNG THÊM CATEGORY
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order cannot be the same as Name.");
            }
            if (obj.Name == null || obj.Name.ToLower() == "")
            {
                ModelState.AddModelError("", "Category Name cannot be empty!");
            }
            if (_repo.Category.GetAll().Any(u => u.Name == obj.Name))
            {
                ModelState.AddModelError("Name", "Category Name already exists!");
            }
            if (ModelState.IsValid)
            {
                _repo.Category.Add(obj);
                _repo.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // CHỨC NĂNG SỬA CATEGORY
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _repo.Category.Get(u=>u.Id_Category==id);
            if(categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order cannot be the same as Name.");
            }
            if (obj.Name == null || obj.Name.ToLower() == "")
            {
                ModelState.AddModelError("", "Category Name cannot be empty!");
            }
            if (_repo.Category.GetAll().Any(u => u.Name == obj.Name))
            {
                ModelState.AddModelError("Name", "Category Name already exists!");
            }
            if (ModelState.IsValid)
            {
                _repo.Category.Update(obj);
                _repo.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // CHỨC NĂNG XÓA CATEGORY
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _repo.Category.Get(u => u.Id_Category == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _repo.Category.Get(u => u.Id_Category == id);
            if (obj == null)
            {
                return NotFound();
            }
            _repo.Category.Remove(obj);
            _repo.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
