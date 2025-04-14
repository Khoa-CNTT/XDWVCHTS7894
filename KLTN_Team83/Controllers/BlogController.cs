using KLTN_Team83.Data;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Controllers
{
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
        public IActionResult CreateBlog()
        {
            return View();
        }
    }
}
