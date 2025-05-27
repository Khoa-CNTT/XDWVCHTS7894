using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Area("Customer")]
    public class HabitsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HabitsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/habits
        // Lấy danh sách thói quen của người dùng hiện tại
        // Thêm query parameter để lọc theo goalId hoặc ngày
        [HttpGet]
        public async Task<IActionResult> GetHabits([FromQuery] int? goalId, [FromQuery] DateTime? date)
        {
            var userId = GetCurrentUserId();
            IEnumerable<Habit> habits;

            if (goalId.HasValue)
            {
                habits =  _unitOfWork.Habit.GetAll(h => h.UserId == userId && h.GoalId == goalId.Value, includeProperties: "Goal,HabitLogs");
            }
            else
            {
                habits =  _unitOfWork.Habit.GetAll(h => h.UserId == userId, includeProperties: "Goal,HabitLogs");
            }

            // Nếu có 'date', bạn có thể tính toán trạng thái hoàn thành cho ngày đó và thêm vào response
            // Ví dụ (cần ViewModel để chứa thêm thông tin này):
            // var habitViewModels = habits.Select(h => new HabitViewModel {
            //     // ... các thuộc tính của Habit ...
            //     IsCompletedToday = date.HasValue ? h.HabitLogs.Any(log => log.LogDate.Date == date.Value.Date && log.IsCompleted) : (bool?)null
            // }).ToList();
            // return Ok(habitViewModels);

            return Ok(habits);
        }

        // GET: api/habits/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabit(int id)
        {
            var userId = GetCurrentUserId();
            var habit =  _unitOfWork.Habit.Get(h => h.Id_Habit == id && h.UserId == userId, includeProperties: "Goal,HabitLogs");
            if (habit == null)
            {
                return NotFound(new { message = "Thói quen không tồn tại hoặc bạn không có quyền truy cập." });
            }
            return Ok(habit);
        }

        // POST: api/habits
        public class HabitCreateModel
        {
            [Required] public string Title { get; set; }
            public string? Description { get; set; }
            public int? GoalId { get; set; }
            public HabitFrequency FrequencyType { get; set; } = HabitFrequency.Daily;
            public List<DayOfWeek>? DaysOfWeek { get; set; } // ["Monday", "Wednesday"]
            public int TargetCompletionsPerPeriod { get; set; } = 1;
            [DataType(DataType.Time)]
            public TimeSpan? ReminderTime { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateHabit([FromBody] HabitCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Kiểm tra xem GoalId (nếu có) có thuộc về user không
            if (model.GoalId.HasValue)
            {
                var goal =  _unitOfWork.Goal.Get(g => g.Id_Goal == model.GoalId.Value && g.UserId == GetCurrentUserId());
                if (goal == null)
                {
                    return BadRequest(new { message = "Mục tiêu không hợp lệ hoặc không thuộc về bạn." });
                }
            }


            var userId = GetCurrentUserId();
            var habit = new Habit
            {
                UserId = userId,
                Title = model.Title,
                Description = model.Description,
                GoalId = model.GoalId,
                FrequencyType = model.FrequencyType,
                DaysOfWeek = (model.FrequencyType == HabitFrequency.Weekly && model.DaysOfWeek != null && model.DaysOfWeek.Any()) ? model.DaysOfWeek : null,
                TargetCompletionsPerPeriod = model.TargetCompletionsPerPeriod > 0 ? model.TargetCompletionsPerPeriod : 1,
                ReminderTime = model.ReminderTime,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

             _unitOfWork.Habit.Add(habit);
             _unitOfWork.Save();
            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id_Habit }, habit);
        }

        // PUT: api/habits/{id}
        // Tương tự CreateHabit nhưng để cập nhật
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabit(int id, [FromBody] HabitCreateModel model) // Tái sử dụng HabitCreateModel hoặc tạo HabitUpdateModel
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var habitFromDb =  _unitOfWork.Habit.Get(h => h.Id_Habit == id && h.UserId == userId);
            if (habitFromDb == null) return NotFound();

            if (model.GoalId.HasValue && model.GoalId != habitFromDb.GoalId)
            { // Kiểm tra GoalId nếu thay đổi
                var goal = _unitOfWork.Goal.Get(g => g.Id_Goal == model.GoalId.Value && g.UserId == GetCurrentUserId());
                if (goal == null)
                {
                    return BadRequest(new { message = "Mục tiêu mới không hợp lệ hoặc không thuộc về bạn." });
                }
            }

            habitFromDb.Title = model.Title;
            habitFromDb.Description = model.Description;
            habitFromDb.GoalId = model.GoalId;
            habitFromDb.FrequencyType = model.FrequencyType;
            habitFromDb.DaysOfWeek = (model.FrequencyType == HabitFrequency.Weekly && model.DaysOfWeek != null && model.DaysOfWeek.Any()) ? model.DaysOfWeek : null;
            habitFromDb.TargetCompletionsPerPeriod = model.TargetCompletionsPerPeriod > 0 ? model.TargetCompletionsPerPeriod : 1;
            habitFromDb.ReminderTime = model.ReminderTime;
            habitFromDb.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Habit.Update(habitFromDb);
            _unitOfWork.Save();
            return NoContent();
        }

        // DELETE: api/habits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var userId = GetCurrentUserId();
            var habitToDelete =  _unitOfWork.Habit.Get(h => h.Id_Habit == id && h.UserId == userId, includeProperties: "HabitLogs");
            if (habitToDelete == null) return NotFound();

            // Xóa các HabitLog liên quan trước
            if (habitToDelete.HabitLogs != null && habitToDelete.HabitLogs.Any())
            {
                _unitOfWork.HabitLog.RemoveRange(habitToDelete.HabitLogs);
            }

            _unitOfWork.Habit.Remove(habitToDelete);
            _unitOfWork.Save();
            return NoContent();
        }

        // --- HabitLog Endpoints ---

        // POST: api/habits/{habitId}/log
        // Ghi nhận hoàn thành thói quen
        public class HabitLogCreateModel
        {
            public DateTime? LogDate { get; set; } // Mặc định là hôm nay nếu null
            public string? Notes { get; set; }
        }
        [HttpPost("{habitId}/log")]
        public async Task<IActionResult> LogHabitCompletion(int habitId, [FromBody] HabitLogCreateModel model)
        {
            var userId = GetCurrentUserId();
            var habit =  _unitOfWork.Habit.Get(h => h.Id_Habit == habitId && h.UserId == userId);
            if (habit == null) return NotFound(new { message = "Thói quen không tồn tại." });

            DateTime logForDate = (model.LogDate ?? DateTime.UtcNow).Date;

            // Kiểm tra xem đã log cho ngày này chưa để tránh trùng lặp (tùy logic bạn muốn)
            var existingLog =  _unitOfWork.HabitLog.Get(hl => hl.Id_Habit == habitId && hl.LogDate == logForDate && hl.UserId == userId);
            if (existingLog != null)
            {
                // Cập nhật nếu đã có hoặc báo lỗi tùy bạn
                existingLog.IsCompleted = true; // Đảm bảo là true
                existingLog.Notes = model.Notes ?? existingLog.Notes;
                _unitOfWork.HabitLog.Update(existingLog);
                _unitOfWork.Save();
                return Ok(existingLog);
                // return Conflict(new { message = "Bạn đã ghi nhận thói quen này cho ngày hôm nay." });
            }


            var habitLog = new HabitLog
            {
                Id_Habit = habitId,
                UserId = userId,
                LogDate = logForDate,
                IsCompleted = true,
                Notes = model.Notes,
                CreatedAt = DateTime.UtcNow
            };
             _unitOfWork.HabitLog.Add(habitLog);
             _unitOfWork.Save();
            return CreatedAtAction(nameof(GetHabitLog), new { habitId = habitId, logId = habitLog.Id }, habitLog);
        }

        // GET: api/habits/{habitId}/log/{logId} - Lấy chi tiết 1 log
        [HttpGet("{habitId}/log/{logId}")]
        public async Task<IActionResult> GetHabitLog(int habitId, int logId)
        {
            var userId = GetCurrentUserId();
            var log =  _unitOfWork.HabitLog.Get(hl => hl.Id == logId && hl.Id_Habit == habitId && hl.UserId == userId);
            if (log == null) return NotFound();
            return Ok(log);
        }


        // DELETE: api/habits/log/{logId}
        // Xóa một lần ghi nhận (nếu đánh dấu nhầm)
        [HttpDelete("log/{logId}")]
        public async Task<IActionResult> DeleteHabitLog(int logId)
        {
            var userId = GetCurrentUserId();
            var logToDelete = _unitOfWork.HabitLog.Get(hl => hl.Id == logId && hl.UserId == userId);
            if (logToDelete == null) return NotFound(new { message = "Không tìm thấy bản ghi." });

            _unitOfWork.HabitLog.Remove(logToDelete);
            _unitOfWork.Save();
            return NoContent();
        }


        // GET: api/habits/calendar?startDate=YYYY-MM-DD&endDate=YYYY-MM-DD
        // Lấy dữ liệu log cho calendar view
        [HttpGet("calendar")]
        public async Task<IActionResult> GetHabitLogsForCalendar([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = GetCurrentUserId();
            // Chỉ lấy các ngày mà có ít nhất 1 thói quen được hoàn thành
            //var completedDates = await _unitOfWork.HabitLog
            //    .GetAllAsync(hl => hl.UserId == userId && hl.IsCompleted && hl.LogDate.Date >= startDate.Date && hl.LogDate.Date <= endDate.Date)
            //    .ContinueWith(task => task.Result.Select(hl => hl.LogDate.Date).Distinct().ToList()); 
            // Dùng ContinueWith nếu GetAllAsync trả Task<IEnumerable>

            // Hoặc nếu GetAllAsync trả về IEnumerable trực tiếp (không async)
            var logsInRange = _unitOfWork.HabitLog.GetAll(hl => hl.UserId == userId && hl.IsCompleted && hl.LogDate.Date >= startDate.Date && hl.LogDate.Date <= endDate.Date);
            var completedDates = logsInRange.Select(hl => hl.LogDate.Date).Distinct().ToList();

            return Ok(completedDates);
        }
    }
}
