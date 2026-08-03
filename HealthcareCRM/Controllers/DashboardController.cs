using HealthcareCRM.Data;
using HealthcareCRM.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public DashboardController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: /Dashboard
        public async Task<IActionResult> Index()
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}