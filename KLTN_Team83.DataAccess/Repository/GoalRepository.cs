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
    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        private ApplicationDbContext _db;
        public GoalRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Goal obj)
        {
            var objFromDb = _db.Goals.FirstOrDefault(u => u.Id_Goal == obj.Id_Goal);
            if (objFromDb != null)
            {
                objFromDb.ApplicationUser.Id = obj.UserId;
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.StartDate = obj.StartDate;
                objFromDb.Status = obj.Status;
                objFromDb.CurrentValue = obj.CurrentValue;
                objFromDb.Unit = obj.Unit;
                objFromDb.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi
                objFromDb.CreatedAt = obj.CreatedAt; // Giữ nguyên thời gian tạo nếu không cần cập nhật
                objFromDb.TargetValue = obj.TargetValue;
                objFromDb.TargetDate = obj.TargetDate;
                objFromDb.Habits = obj.Habits; // Cập nhật danh sách thói quen liên quan nếu cần thiết
                // Nếu cần, có thể cập nhật thêm các thuộc tính khác
            }
        }
    }
    
}
