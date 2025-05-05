using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        void Update(Blog obj);
    }
    
}
