using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.Models;
using KLTN_Team83.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")] // Đánh dấu controller này thuộc area Customer
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);
            var goals = await _context.Goals.Where(g => g.UserId == userId).ToListAsync();
            var habits = await _context.Habits.Where(h => h.UserId == userId).ToListAsync();
            var today = DateTime.Today;
            //var todayHabitEntries = await _context.HabitEntries
                //.Where(e => e.Date == today && habits.Select(h => h.Id_Habit).Contains(e.HabitId))
                //.ToListAsync();
            //var todaySchedule = await _context.ScheduleItems
            //    .Where(s => s.UserId == userId && s.StartTime.Date == today)
            //    .ToListAsync();

            var model = new DashboardVM
            {
                User = (ApplicationUser)user,
                Goals = goals,
                Habits = habits,
                //TodaySchedule = todaySchedule
            };

            return View(model);
        }
    }
}
