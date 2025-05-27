using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KLTN_Team83.Models;
using KLTN_Team83.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using KLTN_Team83.DataAccess.Repository.IRepository;
using System.Security.Claims;
using KLTN_Team83.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Authorize]
    [Route("api/goals")] // Route sẽ là /api/goals
    [ApiController]
    [Area("Customer")] // Đánh dấu controller này thuộc area Customer
    public class GoalsApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty]
        public GoalVM GoalVM { get; set; }

        public GoalsApiController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // Helper để lấy UserId của người dùng hiện tại
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/goals
        [HttpGet] // Mặc định sẽ map tới /api/goals
        public IActionResult GetGoals() // Bỏ async Task nếu UnitOfWork là đồng bộ
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var goals = _unitOfWork.Goal.GetAll(g => g.UserId == userId, includeProperties: "Habits");
            return Ok(goals);
        }

        // GET: api/goals/{id}
        [HttpGet("{id}")] // Map tới /api/goals/{id}
        public IActionResult GetGoal(int id) // Bỏ async Task
        {
            var userId = GetCurrentUserId();
            var goal = _unitOfWork.Goal.Get(g => g.Id_Goal == id && g.UserId == userId, includeProperties: "Habits");

            if (goal == null)
            {
                return NotFound(new { message = "Mục tiêu không tồn tại hoặc bạn không có quyền truy cập." });
            }
            return Ok(goal);
        }

        // POST: api/goals
        // Tạo mục tiêu mới
        // Nên tạo một GoalInputModel/GoalCreateModel để nhận dữ liệu từ client
        public class GoalCreateModel
        {
            [Required(ErrorMessage = "Vui lòng nhập tên mục tiêu.")]
            [StringLength(200)]
            public string Title { get; set; }
            [StringLength(500)]
            public string? Description { get; set; }
            public DateTime TargetDate { get; set; }
            public double? TargetValue { get; set; }
            public string? Unit { get; set; }
        }

        // POST: api/goals
        [HttpPost] // Map tới /api/goals
        public IActionResult CreateGoal([FromBody] GoalCreateModel model) // Bỏ async Task
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var goal = new Goal
            {
                UserId = userId,
                Title = model.Title,
                Description = model.Description,
                TargetDate = model.TargetDate,
                TargetValue = model.TargetValue,
                Unit = model.Unit,
                Status = GoalStatus.InProgress,
                StartDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfWork.Goal.Add(goal);
            _unitOfWork.Save();

            // Quan trọng: GetGoal bây giờ là action của chính controller này
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id_Goal }, goal);
        }

        // PUT: api/goals/{id}
        // Cập nhật mục tiêu
        public class GoalUpdateModel // Tương tự GoalCreateModel nhưng có thể chỉ chứa các trường cho phép sửa
        {
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
            public DateTime TargetDate { get; set; }
            public double? TargetValue { get; set; }
            public double? CurrentValue { get; set; } // Cho phép cập nhật tiến độ ở đây
            public string? Unit { get; set; }
            public GoalStatus? Status { get; set; }
        }

        // PUT: api/goals/{id}
        [HttpPut("{id}")] // Map tới /api/goals/{id}
        public IActionResult UpdateGoal(int id, [FromBody] GoalUpdateModel model) // Bỏ async Task
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var goalFromDb = _unitOfWork.Goal.Get(g => g.Id_Goal == id && g.UserId == userId);

            if (goalFromDb == null) return NotFound(new { message = "Mục tiêu không tồn tại." });

            goalFromDb.Title = model.Title;
            goalFromDb.Description = model.Description;
            goalFromDb.TargetDate = model.TargetDate;
            goalFromDb.TargetValue = model.TargetValue;
            goalFromDb.CurrentValue = model.CurrentValue ?? goalFromDb.CurrentValue;
            goalFromDb.Unit = model.Unit;
            goalFromDb.Status = model.Status ?? goalFromDb.Status;
            goalFromDb.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Goal.Update(goalFromDb);
            _unitOfWork.Save();

            return NoContent();
        }


        // PUT: api/goals/{id}/updateprogress
        // Chỉ cập nhật tiến độ (CurrentValue)
        public class GoalProgressUpdateModel
        {
            [Required]
            public double CurrentValue { get; set; }
        }

        // PUT: api/goals/{id}/updateprogress
        [HttpPut("{id}/updateprogress")] // Map tới /api/goals/{id}/updateprogress
        public IActionResult UpdateGoalProgress(int id, [FromBody] GoalProgressUpdateModel model) // Bỏ async Task
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetCurrentUserId();
            var goalFromDb = _unitOfWork.Goal.Get(g => g.Id_Goal == id && g.UserId == userId);

            if (goalFromDb == null) return NotFound(new { message = "Mục tiêu không tồn tại." });

            goalFromDb.CurrentValue = model.CurrentValue;
            goalFromDb.UpdatedAt = DateTime.UtcNow;
            if (goalFromDb.TargetValue.HasValue && goalFromDb.CurrentValue >= goalFromDb.TargetValue.Value)
            {
                goalFromDb.Status = GoalStatus.Achieved;
            }
            _unitOfWork.Goal.Update(goalFromDb);
            _unitOfWork.Save();
            return Ok(goalFromDb);
        }


        // DELETE: api/goals/{id}
        [HttpDelete("{id}")] // Map tới /api/goals/{id}
        public IActionResult DeleteGoal(int id) // Bỏ async Task
        {
            var userId = GetCurrentUserId();
            var goalToDelete = _unitOfWork.Goal.Get(g => g.Id_Goal == id && g.UserId == userId, includeProperties: "Habits");

            if (goalToDelete == null) return NotFound(new { message = "Mục tiêu không tồn tại." });

            if (goalToDelete.Habits != null && goalToDelete.Habits.Any())
            {
                foreach (var habit in goalToDelete.Habits)
                {
                    habit.GoalId = null;
                    _unitOfWork.Habit.Update(habit);
                }
            }
            _unitOfWork.Goal.Remove(goalToDelete);
            _unitOfWork.Save();
            return NoContent();
        }


        //public async Task<IActionResult> Index()
        //{
        //    // Lấy thông tin người dùng từ ClaimsIdentity
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var goals = _unitOfWork.Goal.GetAll(g => g.UserId == userId);
        //    return View(goals);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Goal obj)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Goal.Add(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Goal created successfully!";
        //        return RedirectToAction("Index");
        //    }
        //   return View();
        //}

        //public async Task<IActionResult> Edit(string? id)
        //{
        //    var goal = _unitOfWork.Goal.Get(g => g.UserId == id);
        //    if (goal == null || goal.UserId != _userManager.GetUserId(User))
        //    {
        //        return NotFound();
        //    }
        //    return View(goal);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, Goal goal)
        //{
        //    if (id != goal.Id_Goal) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            goal.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            _unitOfWork.Goal.Update(goal);
        //            _unitOfWork.Save();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!_unitOfWork.Goal.GetAll().Any(e => e.Id_Goal == goal.Id_Goal)) return NotFound();
        //            throw;
        //        }
        //    }
        //    return View(goal);
        //}

        //public async Task<IActionResult> Delete(string? id)
        //{
        //    var goal = _unitOfWork.Goal.Get(g => g.UserId == id);
        //    if (goal == null || goal.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        //    {
        //        return NotFound();
        //    }
        //    return View(goal);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int? id)
        //{
        //    var goal = _unitOfWork.Goal.Get(g => g.Id_Goal == id);
        //    if (goal != null)
        //    {
        //        _unitOfWork.Goal.Remove(goal);
        //        _unitOfWork.Save();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}