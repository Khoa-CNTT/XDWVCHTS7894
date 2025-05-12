using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KLTN_Team83.Models;
using KLTN_Team83.DataAccess.Data;
using Microsoft.AspNetCore.Identity;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Authorize]
    public class GoalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var goals = await _context.Goals
                .Where(g => g.UserId == userId)
                .ToListAsync();
            return View(goals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                goal.UserId = _userManager.GetUserId(User);
                _context.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goal);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null || goal.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Goal goal)
        {
            if (id != goal.Id_Goal) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    goal.UserId = _userManager.GetUserId(User);
                    _context.Update(goal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Goals.Any(e => e.Id_Goal == goal.Id_Goal)) return NotFound();
                    throw;
                }
            }
            return View(goal);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null || goal.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(goal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}