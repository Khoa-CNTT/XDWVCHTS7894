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
    public class HabitRepository : Repository<Habit>, IHabitRepository
    {
        private ApplicationDbContext _db;
        public HabitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Habit obj)
        {
            var objFromDb = _db.Habits.FirstOrDefault(u => u.Id_Habit == obj.Id_Habit);
            if (objFromDb != null)
            {
                objFromDb.ApplicationUser.Id = obj.UserId;
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.FrequencyType = obj.FrequencyType;
                objFromDb.DaysOfWeek = obj.DaysOfWeek;
                objFromDb.TargetCompletionsPerPeriod = obj.TargetCompletionsPerPeriod;
                objFromDb.ReminderTime = obj.ReminderTime;
                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.UpdatedAt = obj.UpdatedAt;
                // Cập nhật các thuộc tính liên quan đến mục tiêu nếu có
                if (obj.Goal != null)
                {
                    objFromDb.GoalId = obj.Goal.Id_Goal;
                    objFromDb.Goal.Title = obj.Goal.Title;
                    objFromDb.Goal.Description = obj.Goal.Description;
                    objFromDb.Goal.TargetValue = obj.Goal.TargetValue;
                    objFromDb.Goal.TargetDate = obj.Goal.TargetDate;
                }
                // Cập nhật các thuộc tính liên quan đến nhật ký thói quen nếu có
                objFromDb.HabitLogs = obj.HabitLogs.Select(log => new HabitLog
                {
                    Id = log.Id,
                    Id_Habit = log.Id_Habit,
                    UserId = log.UserId,
                    LogDate = log.LogDate,
                    IsCompleted = log.IsCompleted,
                    Notes = log.Notes,
                    CreatedAt = log.CreatedAt
                }).ToList();
            }
        }
    }
    
}
