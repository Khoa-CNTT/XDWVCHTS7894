using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id_Product == obj.Id_Product);
            if(objFromDb != null)
            {
                objFromDb.NameProduct = obj.NameProduct;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.NCC = obj.NCC;
                objFromDb.Id_Category = obj.Id_Category;
                objFromDb.ProductImages =obj.ProductImages;
                //if (obj.ImgageUrl != null)
                //{
                //    objFromDb.ImgageUrl = obj.ImgageUrl;
                //}
            }
        }
    }
    
}
