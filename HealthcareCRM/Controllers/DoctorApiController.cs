using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
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

        // GET: api/doctors
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

        // POST: api/doctors
        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            var doctor = new Doctor
            {
                Name = model.Name,
                Specialization = model.Specialization,
                Phone = model.Phone,
                ScheduleDays = model.ScheduleDays,
                IsActive = true
            };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = doctor,
                message = "Doctor created"
            });
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorViewModel model)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound(new { success = false, message = "Doctor not found" });

            doctor.Name = model.Name;
            doctor.Specialization = model.Specialization;
            doctor.Phone = model.Phone;
            doctor.ScheduleDays = model.ScheduleDays;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = doctor,
                message = "Doctor updated"
            });
        }

        // PUT: api/doctors/5/deactivate
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound(new { success = false, message = "Doctor not found" });

            // Soft delete - toggle active status
            doctor.IsActive = !doctor.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = doctor,
                message = doctor.IsActive ? "Doctor reactivated" : "Doctor deactivated"
            });
        }
    }
}
