using HealthcareCRM.Data;
using HealthcareCRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() =>
            HttpContext.Session.GetString("UserRole") == "Admin";

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // POST: /Admin/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var oldRole = user.Role;
            user.Role = role;
            await _context.SaveChangesAsync();

            // Log the action
            await LogAction("ChangeRole", "User", id,
                $"Changed role from {oldRole} to {role} for {user.FullName}");

            TempData["Success"] = $"Role updated to {role} successfully.";
            return RedirectToAction("Index");
        }

        // POST: /Admin/ToggleActive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            // Log the action
            await LogAction(user.IsActive ? "Activate" : "Deactivate", "User", id,
                $"{(user.IsActive ? "Activated" : "Deactivated")} account for {user.FullName}");

            TempData["Success"] = user.IsActive
                ? "User account activated."
                : "User account deactivated.";
            return RedirectToAction("Index");
        }

        // GET: /Admin/AuditLog
        public async Task<IActionResult> AuditLog()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return View(logs);
        }

        // Helper: Log admin actions
        private async Task LogAction(string action, string targetType, int targetId, string details)
        {
            var adminId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var log = new AuditLog
            {
                UserId = adminId,
                Action = action,
                TargetType = targetType,
                TargetId = targetId,
                Details = details
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}