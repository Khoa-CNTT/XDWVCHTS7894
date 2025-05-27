using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private ApplicationDbContext _db;
        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Blog obj)
        {
            var objFromDb = _db.Blogs.FirstOrDefault(u => u.id_Blog == obj.id_Blog);
            if (objFromDb != null)
            {
                objFromDb.tilte = obj.tilte;
                objFromDb.content = obj.content;
                objFromDb.id_TypeBlog = obj.id_TypeBlog;
                objFromDb.ngayTao = obj.ngayTao;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
        
        
    }
    
}
