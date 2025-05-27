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
    public class HabitLogRepository : Repository<HabitLog>, IHabitLogRepository
    {
        private ApplicationDbContext _db;
        public HabitLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(HabitLog obj)
        {
            var objFromDb = _db.HabitLogs.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ApplicationUser.Id = obj.UserId;
                objFromDb.Id_Habit = obj.Id_Habit;
                objFromDb.LogDate = obj.LogDate;
                objFromDb.IsCompleted = obj.IsCompleted;
                objFromDb.Notes = obj.Notes;
                objFromDb.CreatedAt = obj.CreatedAt;
                // Nếu cần cập nhật thêm các thuộc tính khác, có thể thêm vào đây
                _db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }
    }
    
}
