using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository.IRepository
{
    public interface IAccountRepository: IRepository<Account>
    {
        void Update(Account obj);
        //Task<Account> GetByIdAsync(int id);
        //Task<IEnumerable<Account>> GetAllAsync();
        //Task AddAsync(Account obj);
        //Task RemoveAsync(Account obj);
        //Task RemoveRangeAsync(IEnumerable<Account> obj);
    }
    
}
