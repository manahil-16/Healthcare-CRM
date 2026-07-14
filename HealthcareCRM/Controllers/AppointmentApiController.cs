using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<IActionResult> GetAll(string? status)
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                appointments = appointments.Where(a => a.Status == status);

            var result = await appointments.ToListAsync();

            return Ok(new
            {
                success = true,
                data = result.Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Status,
                    a.Notes,
                    PatientName = a.Patient!.FullName,
                    DoctorName = a.Doctor!.Name
                }),
                message = "Appointments retrieved"
            });
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            var appointment = new Appointment
            {
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                AppointmentDate = model.AppointmentDate,
                Status = "Pending",
                Notes = model.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new
            {
                success = true,
                data = appointment,
                message = "Appointment created"
            });
        }

        // PUT: api/appointments/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound(new { success = false, message = "Appointment not found" });

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = appointment,
                message = "Status updated"
            });
        }

        // PUT: api/appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AppointmentViewModel model)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound(new { success = false, message = "Appointment not found" });

            appointment.PatientId = model.PatientId;
            appointment.DoctorId = model.DoctorId;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = appointment,
                message = "Appointment updated"
            });
        }
    }
}