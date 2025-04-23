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
    public class TypeBlogRepository : Repository<TypeBlog>, ITypeBlogRepository
    {
        private ApplicationDbContext _db;
        public TypeBlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(TypeBlog obj)
        {
            _db.TypeBlogs.Update(obj);
        }
        
        
    }
    
}
