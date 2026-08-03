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
public async Task<IActionResult> GetAll(
    string? search,
    int page = 1,
    int pageSize = 20)
{
    page = Math.Max(1, page);
    pageSize = Math.Clamp(pageSize, 1, 100);
    var query = _context.Patients.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(p =>
            p.FullName.Contains(search) ||
            p.Phone.Contains(search) ||
            p.DateOfBirth.ToString().Contains(search));
    }

    int totalRecords = await query.CountAsync();

    var patients = await query
        .OrderBy(p => p.FullName)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new
    {
        success = true,
        data = patients,
        page,
        pageSize,
        totalRecords,
        totalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
        message = "Patients retrieved successfully."
    });
}

        // GET: api/patients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { success = false, message = "Patient not found" });

            return Ok(new { success = true, data = patient, message = "Patient retrieved" });
        }

        // POST: api/patients
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

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

        // PUT: api/patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, data = (object?)null, message = "Invalid patient data." });
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { success = false, message = "Patient not found" });

            patient.FullName = model.FullName;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Address = model.Address;
            patient.DateOfBirth = model.DateOfBirth;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, data = patient, message = "Patient updated" });
        }

        // DELETE: api/patients/5
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    var patient = await _context.Patients.FindAsync(id);

    if (patient == null)
    {
        return NotFound(new
        {
            success = false,
            data = (object?)null,
            message = "Patient not found."
        });
    }

    _context.Patients.Remove(patient);

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
        return Conflict(new { success = false, data = (object?)null, message = "Patient cannot be deleted while appointments are linked to it." });
    }

    return Ok(new
    {
        success = true,
        data = (object?)null,
        message = "Patient deleted successfully."
    });
}
    }
}
