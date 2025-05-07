using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository.IRepository;

namespace KLTN_Team83.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IAccountRepository Account { get; private set; }
        public IBlogRepository Blog { get; private set; }
        public ITypeBlogRepository TypeBlog { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Account = new AccountRepository(_db);
            Blog = new BlogRepository(_db);
            TypeBlog = new TypeBlogRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
        }
        
        public void Save() {
            _db.SaveChanges();
        }
    }
}
