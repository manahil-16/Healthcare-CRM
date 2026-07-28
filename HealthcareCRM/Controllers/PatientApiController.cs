using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/patients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var patients = await _context.Patients.ToListAsync();
                return Ok(new { success = true, data = patients, message = "Patients retrieved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // GET: api/patients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return NotFound(new { success = false, message = "Patient not found", data = (object?)null });

                return Ok(new { success = true, data = patient, message = "Patient retrieved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // POST: api/patients
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid data", data = (object?)null });

                var patient = new Patient
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = patient.Id },
                    new { success = true, data = patient, message = "Patient created" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // PUT: api/patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientViewModel model)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return NotFound(new { success = false, message = "Patient not found", data = (object?)null });

                patient.FullName = model.FullName;
                patient.Email = model.Email;
                patient.Phone = model.Phone;
                patient.Address = model.Address;
                patient.DateOfBirth = model.DateOfBirth;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, data = patient, message = "Patient updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        // DELETE: api/patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return NotFound(new { success = false, message = "Patient not found", data = (object?)null });

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Patient deleted successfully", data = (object?)null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, data = (object?)null });
            }
        }
    }
}