using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
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

        // GET: /Patient
        public async Task<IActionResult> Index(string search)
        {
            var patients = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                patients = patients.Where(p => p.FullName.Contains(search));

            ViewBag.Search = search;
            return View(await patients.ToListAsync());
        }

        // GET: /Patient/Create
        public IActionResult Create()
        {
            return View(new PatientViewModel());
        }

        // POST: /Patient/Create
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
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
        }

        // GET: /Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}