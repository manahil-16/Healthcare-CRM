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

        // Check if admin
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
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Role = role;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Role updated to {role} successfully.";
            return RedirectToAction("Index");
        }
    }
}