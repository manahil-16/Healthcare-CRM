using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using HealthcareCRM.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public PatientController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: /Patient
        // GET: /Patient
public async Task<IActionResult> Index(string search, int page = 1)
{
    if (!AuthHelper.IsAuthenticated(_accessor))
        return RedirectToAction("Login", "Account");

    const int pageSize = 20;

    var query = _context.Patients.AsQueryable();

    // Search by name, phone, or date of birth
    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();

        query = query.Where(p =>
            p.FullName.Contains(search) ||
            p.Phone.Contains(search));
    }

    // Try searching by DOB if user entered a valid date
    if (DateTime.TryParse(search, out DateTime dob))
    {
        query = query.Where(p => p.DateOfBirth.Date == dob.Date);
    }

    int totalRecords = await query.CountAsync();

    var patients = await query
        .OrderBy(p => p.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewBag.Search = search;
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    ViewBag.TotalRecords = totalRecords;

    return View(patients);
}

        // GET: /Patient/Create
        public IActionResult Create()
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            return View(new PatientViewModel());
        }

        // POST: /Patient/Create
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            ModelState.Remove("Id");

            if (!ModelState.IsValid) return View(model);

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
            return RedirectToAction("Index");
        }

        // GET: /Patient/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var model = new PatientViewModel
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

        // POST: /Patient/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(PatientViewModel model)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            ModelState.Remove("Id");

            if (!ModelState.IsValid) return View(model);

            var patient = await _context.Patients.FindAsync(model.Id);
            if (patient == null) return NotFound();

            patient.FullName = model.FullName;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Address = model.Address;
            patient.DateOfBirth = model.DateOfBirth;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }// GET: /Patient/Details/5
public async Task<IActionResult> Details(int id)
{
    if (!AuthHelper.IsAuthenticated(_accessor))
        return RedirectToAction("Login", "Account");

    var patient = await _context.Patients.FindAsync(id);

    if (patient == null)
        return NotFound();

    return View(patient);
}

        // GET: /Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}