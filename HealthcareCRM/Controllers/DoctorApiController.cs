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
            try
            {
                var doctors = await _context.Doctors
                    .Where(d => d.IsActive)
                    .ToListAsync();

                return Ok(new { success = true, data = doctors, message = "Doctors retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // POST: api/doctors
        [HttpPost]
        public async Task<IActionResult> Create(Doctor model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid data", data = (object?)null });

                _context.Doctors.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = model, message = "Doctor created" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor model)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                    return NotFound(new { success = false, message = "Doctor not found", data = (object?)null });

                doctor.Name = model.Name;
                doctor.Specialization = model.Specialization;
                doctor.Phone = model.Phone;
                doctor.ScheduleDays = model.ScheduleDays;

                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = doctor, message = "Doctor updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // PUT: api/doctors/5/deactivate
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                    return NotFound(new { success = false, message = "Doctor not found", data = (object?)null });

                doctor.IsActive = !doctor.IsActive;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = doctor,
                    message = doctor.IsActive ? "Doctor reactivated" : "Doctor deactivated"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }
    }
}