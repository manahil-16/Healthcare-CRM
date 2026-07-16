using Microsoft.AspNetCore.Mvc;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using HealthcareCRM.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public DoctorController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: /Doctor
        public async Task<IActionResult> Index()
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var doctors = await _context.Doctors.ToListAsync();
            return View(doctors);
        }

        // GET: /Doctor/Create
        public IActionResult Create()
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            return View(new DoctorViewModel());
        }

        // POST: /Doctor/Create
        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel model)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View(model);

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
            return RedirectToAction("Index");
        }

        // GET: /Doctor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            var model = new DoctorViewModel
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Phone = doctor.Phone,
                ScheduleDays = doctor.ScheduleDays,
                IsActive = doctor.IsActive
            };

            return View(model);
        }

        // POST: /Doctor/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(DoctorViewModel model)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            ModelState.Remove("Id");
            if (!ModelState.IsValid) return View(model);

            var doctor = await _context.Doctors.FindAsync(model.Id);
            if (doctor == null) return NotFound();

            doctor.Name = model.Name;
            doctor.Specialization = model.Specialization;
            doctor.Phone = model.Phone;
            doctor.ScheduleDays = model.ScheduleDays;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            if (!AuthHelper.IsAuthenticated(_accessor))
                return RedirectToAction("Login", "Account");

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            // Soft delete — toggle
            doctor.IsActive = !doctor.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
