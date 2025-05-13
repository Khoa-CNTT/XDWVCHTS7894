using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KLTN_Team83.Models;
using KLTN_Team83.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using KLTN_Team83.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace KLTN_Team83.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")] // Đánh dấu controller này thuộc area Customer
    public class GoalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public GoalController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goals = _unitOfWork.Goal.Get(g => g.UserId == userId);
            return View(goals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Goal obj)
        {
            if (ModelState.IsValid)
            {
                obj.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _unitOfWork.Goal.Add(obj);
                _unitOfWork.Goal.Save();
                TempData["success"] = "Goal created successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            var goal = _unitOfWork.Goal.Get(g => g.UserId == id);
            if (goal == null || goal.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Goal goal)
        {
            if (id != goal.Id_Goal) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    goal.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _unitOfWork.Goal.Update(goal);
                    _unitOfWork.Goal.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_unitOfWork.Goal.GetAll().Any(e => e.Id_Goal == goal.Id_Goal)) return NotFound();
                    throw;
                }
            }
            return View(goal);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            var goal = _unitOfWork.Goal.Get(g => g.UserId == id);
            if (goal == null || goal.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            return View(goal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var goal = _unitOfWork.Goal.Get(g => g.Id_Goal == id);
            if (goal != null)
            {
                _unitOfWork.Goal.Remove(goal);
                _unitOfWork.Goal.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}