using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/doctors — list all active doctors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _context.Doctors
                .Where(d => d.IsActive)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = doctors,
                message = "Doctors retrieved successfully"
            });
        }
    }
}