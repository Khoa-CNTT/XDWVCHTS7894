using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser obj)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == obj.Id);
            objFromDb.Name = obj.Name;
            objFromDb.Height = obj.Height;
            objFromDb.Weight = obj.Weight;
            objFromDb.Age = obj.Age;
            objFromDb.Gender = obj.Gender;
            objFromDb.HealthGoal = obj.HealthGoal;
            objFromDb.Id_Company = obj.Id_Company;
            objFromDb.Role = obj.Role;
        }

    }
    
}
