using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using KLTN_Team83.Models.ViewModels;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _db.Product.GetAll().ToList();
            
            return View(objProductList);
        }

        // CHỨC NĂNG THÊM TYPEBLOG
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _db.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id_Category.ToString()
                }),
                Product = new Product()
            };

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Add(productVM.Product);
                _db.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _db.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id_Category.ToString()
                });
                    return View(productVM);
            }
        }

        // CHỨC NĂNG SỬA TYPEBLOG
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _db.Product.Get(u=>u.Id_Product==id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);

        }
        [HttpPost]
        public IActionResult Edit(ProductVM productVM)
        {
            if (productVM.Product.NameProduct == null || productVM.Product.NameProduct.ToLower() == "")
            {
                ModelState.AddModelError("", "NameProduct cannot be empty!");
            }
            if (_db.Product.GetAll().Any(u => u.NameProduct == productVM.Product.NameProduct))
            {
                ModelState.AddModelError("NameProduct", "NameProduct already exists!");
            }
            if (ModelState.IsValid)
            {
                _db.Product.Update(productVM.Product);
                _db.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _db.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id_Category.ToString()
                });
                return View(productVM);
            }
        }

        // CHỨC NĂNG XÓA TYPEBLOG
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _db.Product.Get(u => u.Id_Product == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _db.Product.Get(u => u.Id_Product == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Product.Remove(obj);
            _db.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
