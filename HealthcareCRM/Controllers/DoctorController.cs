using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public DoctorController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: /Doctor
        public async Task<IActionResult> Index()
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var doctors = await _context.Doctors
                .Where(d => d.IsActive)
                .ToListAsync();

            return View(doctors);
        }
    }
}