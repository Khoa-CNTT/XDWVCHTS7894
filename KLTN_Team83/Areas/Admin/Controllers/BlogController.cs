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
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public BlogController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Blog> objBlogList = _db.Blog.GetAll().ToList();
            
            return View(objBlogList);
        }

        // CHỨC NĂNG THÊM BLOG
        public IActionResult Upsert(int? id)
        {
            BlogVM blogVM = new()
            {
                TypeBlogList = _db.TypeBlog.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.id_TypeBlog.ToString()
                }),
                Blog = new Blog()
            };
            blogVM.Blog.ngayTao = DateTime.Now;
            if (id == null || id == 0)
            {
                blogVM.Blog.ngayTao = DateTime.Now;
                //create
                return View(blogVM);
            }
            else
            {
                blogVM.Blog.ngayTao = DateTime.Now;
                //update
                blogVM.Blog = _db.Blog.Get(u => u.id_Blog == id);
                return View(blogVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(BlogVM blogVM, IFormFile? file)
        {
            blogVM.Blog.ngayTao = DateTime.Now;
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string uploads = Path.Combine(wwwRootPath, @"images\blog");
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    if (!string.IsNullOrEmpty(blogVM.Blog.ImageUrl))
                    {
                        blogVM.Blog.ngayTao = DateTime.Now;
                        //delete old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, blogVM.Blog.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    blogVM.Blog.ImageUrl = @"\images\blog\" + fileName;
                }
                blogVM.Blog.ngayTao = DateTime.Now;
                if (blogVM.Blog.id_Blog == 0)
                {
                    blogVM.Blog.ngayTao = DateTime.Now;
                    _db.Blog.Add(blogVM.Blog);
                }
                else
                {
                    blogVM.Blog.ngayTao = DateTime.Now;
                    _db.Blog.Update(blogVM.Blog);
                }
                blogVM.Blog.ngayTao = DateTime.Now;
                _db.Save();
                TempData["success"] = "Blog created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                blogVM.TypeBlogList = _db.TypeBlog.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.id_TypeBlog.ToString()
                });
                return View(blogVM);
            }
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Blog> objBlogList = _db.Blog.GetAll(includeProperties: "TypeBlog").ToList();
            return Json(new { data = objBlogList });
        }

        // CHỨC NĂNG XÓA BLOG
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var blogToBeDelete = _db.Blog.Get(u => u.id_Blog == id);
            if (blogToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while Delete" });
            }
            //xóa ảnh cũ
            var oldImagePath =
                            Path.Combine(_hostEnvironment.WebRootPath,
                            blogToBeDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _db.Blog.Remove(blogToBeDelete);
            _db.Save();

            List<Blog> objBlogList = _db.Blog.GetAll(includeProperties: "TypeBlog").ToList();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
