using HealthcareCRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            var totalPatients = await _context.Patients.CountAsync();
            var totalDoctors = await _context.Doctors
                .Where(d => d.IsActive).CountAsync();
            var appointmentsToday = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == today).CountAsync();
            var appointmentsThisWeek = await _context.Appointments
                .Where(a => a.AppointmentDate >= weekStart
                    && a.AppointmentDate < weekEnd).CountAsync();
            var pendingCount = await _context.Appointments
                .Where(a => a.Status == "Pending").CountAsync();
            var confirmedCount = await _context.Appointments
                .Where(a => a.Status == "Confirmed").CountAsync();
            var cancelledCount = await _context.Appointments
                .Where(a => a.Status == "Cancelled").CountAsync();

            return Ok(new
            {
                success = true,
                message = "Dashboard stats retrieved",
                data = new
                {
                    totalPatients,
                    totalDoctors,
                    appointmentsToday,
                    appointmentsThisWeek,
                    pendingCount,
                    confirmedCount,
                    cancelledCount
                }
            });
        }
    }
}