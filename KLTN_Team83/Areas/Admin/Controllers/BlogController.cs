using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BlogController(ApplicationDbContext db   )
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> objBlogList = await _db.Blogs.ToListAsync();
            return View(objBlogList);
        }

        // CHỨC NĂNG THÊM BLOG
        public IActionResult Create()
        {
            return View();
        }
        //Thêm blog vào database
        [HttpPost]
        public IActionResult Create(Blog obj)
        {
            ////kiểm tra có giống nhau hay ko
            //if (obj.tilte == obj.content)
            //{
            //    ModelState.AddModelError("tilte", "Mật khẩu không được trùng với tên đăng nhập.");
            //}
            //obj.ngayTao = DateTime.Now;

            //ktra tiêu đề có giống với tiêu đề đã có hay ko
            if (obj.tilte!=null && obj.tilte.ToLower() == "khánh")
            {
                ModelState.AddModelError("", "Tiêu đề đã được dùng!");
            }
            
            obj.ngayTao = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.Blogs.Add(obj);
                _db.SaveChanges();
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
            Blog? blogFromDb = _db.Blogs.Find(id);
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
                _db.Blogs.Update(obj);
                _db.SaveChanges();
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
            Blog? blogFromDb = _db.Blogs.Find(id);
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
            Blog? obj = _db.Blogs.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Blogs.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Blog");
            
        }
    }
}
