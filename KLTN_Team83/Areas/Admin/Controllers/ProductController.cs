using Google.Ai.Generativelanguage.V1Beta2;
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
using Microsoft.IdentityModel.Tokens;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _db.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(objProductList);
        }

        // CHỨC NĂNG THÊM PRODUCT
        public IActionResult Upsert( int? id)
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
            if(id==null|| id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _db.Product.Get(u => u.Id_Product == id, includeProperties: "ProductImages");
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id_Product == 0)
                {
                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    _db.Product.Update(productVM.Product);
                }

                _db.Save();

                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
                        string productPath = @"images\products\product-" + productVM.Product.Id_Product;
                        string uploads = Path.Combine(wwwRootPath, productPath);

                        if(!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new ProductImage()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            Id_Product = productVM.Product.Id_Product,
                        };

                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                        
                    }
                    _db.Product.Update(productVM.Product);
                    _db.Save();
                }
                
                TempData["success"] = "Product created/update successfully!";
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

        public IActionResult DeleteImg (int imageId)
        {
            var imageToBeDelete = _db.ProductImage.Get(u => u.Id_ProductImg == imageId);
            int productId = imageToBeDelete.Id_Product;
            if (imageToBeDelete != null)
            {
                if(!string.IsNullOrEmpty(imageToBeDelete.ImageUrl))
                {
                    //xóa ảnh cũ
                    var oldImagePath =
                                    Path.Combine(_hostEnvironment.WebRootPath,
                                    imageToBeDelete.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _db.ProductImage.Remove(imageToBeDelete);
                _db.Save();
                TempData["success"] = "Image deleted successfully!";
            }
            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> objProductList = _db.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        // CHỨC NĂNG XÓA PRODUCT
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDelete = _db.Product.Get(u => u.Id_Product == id);
            if (productToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while Delete" });
            }
            //xóa toàn bộ ảnh
            string productPath = @"images\products\product-" + id;
            string uploads = Path.Combine(_hostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(uploads))
            {
                string[] filePaths = Directory.GetFiles(uploads);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(uploads);
            }

            _db.Product.Remove(productToBeDelete);
            _db.Save();

            List<Product> objProductList = _db.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
