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
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Blog> objBlogList = _unitOfWork.Blog.GetAll().ToList();
            
            return View(objBlogList);
        }

        // CHỨC NĂNG THÊM BLOG
        public IActionResult Create(IFormFile? file )
        {
            IEnumerable<SelectListItem> TypeBlogList = _unitOfWork.TypeBlog.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.id_TypeBlog.ToString()
            });

            ViewBag.TypeBlogList = TypeBlogList;
            //ViewData["TypeBlogList"] = TypeBlogList;
            return View();
        }
        //Thêm blog vào database
        [HttpPost]
        public IActionResult Create(Blog obj, IFormFile? file)
        {
            ////kiểm tra có giống nhau hay ko
            //if (obj.tilte == obj.content)
            //{
            //    ModelState.AddModelError("tilte", "Mật khẩu không được trùng với tên đăng nhập.");
            //}
            //obj.ngayTao = DateTime.Now;

            //ktra tiêu đề có giống với tiêu đề đã có hay ko
            if (obj.tilte!=null && obj.tilte.ToLower() == "")
            {
                ModelState.AddModelError("", "Tiêu đề đã được dùng!");
            }
            
            obj.ngayTao = DateTime.Now;
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string upload = Path.Combine(wwwRootPath, @"images\blog");

                    if(!string.IsNullOrEmpty(obj.img))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.img = @"\images\blog\" + fileName;
                }
                if(obj.id_Blog == 0)
                {
                    _unitOfWork.Blog.Add(obj);
                }
                else
                {
                    _unitOfWork.Blog.Update(obj);
                }


                _unitOfWork.Blog.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Blog created successfully";
                return RedirectToAction("Index", "Blog");
            }
            return View();
        }

        // CHỨC NĂNG SỬA BLOG
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Blog? blogFromDb = _unitOfWork.Blog.Get(u=>u.id_Blog==id);
            //Blog? blogFromDb1 = _db.Blogs.FirstOrDefault(u => u.id_Blog == id);
            //Blog? blogFromDb2 = _db.Blogs.Where(u => u.id_Blog == id).FirstOrDefault();
            if (blogFromDb == null)
            {
                return NotFound();
            }
            return View(blogFromDb);
        }
        //Thêm blog vào database
        [HttpPost]
        public IActionResult Edit(Blog obj)
        {
            obj.ngayTao = DateTime.Now;
            if (ModelState.IsValid)
            {
                _unitOfWork.Blog.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Blog update successfully";
                return RedirectToAction("Index", "Blog");
            }
            return View();
        }


        // CHỨC NĂNG XÓA BLOG
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Blog? blogFromDb = _unitOfWork.Blog.Get(u => u.id_Blog == id);
            if (blogFromDb == null)
            {
                return NotFound();
            }
            return View(blogFromDb);
        }
        //Thêm blog vào database
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Blog? obj = _unitOfWork.Blog.Get(u => u.id_Blog == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Blog.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Blog");
            
        }
    }
}
