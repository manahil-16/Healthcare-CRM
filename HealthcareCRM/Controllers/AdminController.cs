using System.Text;
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

        private async Task LogAuditAsync(string action, string entityType, int? entityId, string details)
        {
            var currentUserName = HttpContext.Session.GetString("UserName") ?? "System";
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            _context.AuditLogs.Add(new AuditLog
            {
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                PerformedBy = currentUserName,
                PerformedByUserId = currentUserId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var users = await _context.Users.OrderBy(u => u.FullName).ToListAsync();
            ViewBag.RecentAuditLogs = await _context.AuditLogs
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Role = role;
            await _context.SaveChangesAsync();
            await LogAuditAsync("RoleUpdated", "User", user.Id, $"Role changed to {role} for {user.Email}.");

            TempData["Success"] = $"Role updated to {role} successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
            await LogAuditAsync(user.IsActive ? "UserActivated" : "UserDeactivated", "User", user.Id, $"User account for {user.Email} was {(user.IsActive ? "activated" : "deactivated")}.");

            TempData["Success"] = user.IsActive ? "User account activated." : "User account deactivated.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportUsers()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var users = await _context.Users.OrderBy(u => u.FullName).ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Id,FullName,Email,Role,IsActive,CreatedAt");

            foreach (var user in users)
            {
                csv.AppendLine($"{user.Id},{EscapeCsv(user.FullName)},{EscapeCsv(user.Email)},{EscapeCsv(user.Role)},{user.IsActive},{user.CreatedAt:O}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "users-export.csv");
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var escaped = value.Replace("\"", "\"\"");
            return escaped.Contains(',') || escaped.Contains('"') || escaped.Contains('\n') || escaped.Contains('\r')
                ? $"\"{escaped}\""
                : escaped;
        }
    }
}