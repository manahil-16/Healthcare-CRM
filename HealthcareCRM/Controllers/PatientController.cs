using HealthcareCRM.Data;
using HealthcareCRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> HasAppointments(int patientId) =>
            await _context.Appointments.AnyAsync(a => a.PatientId == patientId);

        // Patient List + Search
        // GET: /Patient
public async Task<IActionResult> Index(string? search, int page = 1)
{
    int pageSize = 20;

    var query = _context.Patients.AsQueryable();

    // Search by Name, Phone or Date of Birth
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(p =>
            p.FullName.Contains(search) ||
            p.Phone.Contains(search) ||
            p.DateOfBirth.ToString().Contains(search));
    }

    int totalRecords = await query.CountAsync();
    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    var patients = await query
        .OrderBy(p => p.FullName)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewBag.Search = search;
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;

    return View(patients);
}
        // Details
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // GET
        public IActionResult Create()
        {
            return View(new PatientViewModel());
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Patient patient = new()
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth
            };

            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            PatientViewModel model = new()
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth
            };

            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PatientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var patient = await _context.Patients.FindAsync(model.Id);

            if (patient == null)
                return NotFound();

            patient.FullName = model.FullName;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Address = model.Address;
            patient.DateOfBirth = model.DateOfBirth;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       // =======================
// GET: Patient/Delete/5
// =======================
public async Task<IActionResult> Delete(int id)
{
    var patient = await _context.Patients.FindAsync(id);

    if (patient == null)
        return NotFound();

    return View(patient);
}

// =======================
// POST: Patient/Delete/5
// =======================
[HttpPost]
[ValidateAntiForgeryToken]
[ActionName("Delete")]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var patient = await _context.Patients.FindAsync(id);

    if (patient == null)
        return NotFound();

    if (await HasAppointments(id))
    {
        TempData["Error"] = "This patient cannot be deleted because appointments are linked to the record.";
        return RedirectToAction(nameof(Details), new { id });
    }

    _context.Patients.Remove(patient);

    await _context.SaveChangesAsync();

    TempData["Success"] = "Patient deleted successfully.";

    return RedirectToAction(nameof(Index));
}
    }
}
