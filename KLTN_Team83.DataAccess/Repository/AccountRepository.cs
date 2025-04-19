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
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(Account obj)
        {
            _db.Accounts.Update(obj);
        }
        
    }
    
}
