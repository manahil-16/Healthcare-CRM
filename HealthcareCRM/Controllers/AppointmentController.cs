using HealthcareCRM.Data;
using HealthcareCRM.Helpers;
using HealthcareCRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Controllers;

public class AppointmentController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public AppointmentController(AppDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }

    private bool IsAuthenticated() => AuthHelper.IsAuthenticated(_accessor);

    public async Task<IActionResult> Index(string? status)
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        var query = _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(a => a.Status == status);
        ViewBag.Status = status;
        return View(await query.OrderByDescending(a => a.AppointmentDate).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        return View(await NewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentViewModel model)
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        if (!await ValidSelection(model)) ModelState.AddModelError(string.Empty, "Select an existing patient and active doctor.");
        if (!ModelState.IsValid) return View(await NewModel(model));
        _context.Appointments.Add(new Appointment { PatientId = model.PatientId, DoctorId = model.DoctorId, AppointmentDate = model.AppointmentDate, Notes = model.Notes, Status = "Pending" });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Appointment booked successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();
        return View(await NewModel(new AppointmentViewModel { Id = appointment.Id, PatientId = appointment.PatientId, DoctorId = appointment.DoctorId, AppointmentDate = appointment.AppointmentDate, Notes = appointment.Notes, Status = appointment.Status }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AppointmentViewModel model)
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        if (!await ValidSelection(model)) ModelState.AddModelError(string.Empty, "Select an existing patient and active doctor.");
        if (!ModelState.IsValid) return View(await NewModel(model));
        var appointment = await _context.Appointments.FindAsync(model.Id);
        if (appointment is null) return NotFound();
        appointment.PatientId = model.PatientId; appointment.DoctorId = model.DoctorId; appointment.AppointmentDate = model.AppointmentDate; appointment.Notes = model.Notes;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Appointment updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        if (!IsAuthenticated()) return RedirectToAction("Login", "Account");
        if (status is not ("Pending" or "Confirmed" or "Cancelled")) return BadRequest();
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();
        appointment.Status = status; await _context.SaveChangesAsync();
        TempData["Success"] = "Appointment status updated.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ValidSelection(AppointmentViewModel model) =>
        await _context.Patients.AnyAsync(p => p.Id == model.PatientId) && await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId && d.IsActive);

    private async Task<AppointmentViewModel> NewModel(AppointmentViewModel? model = null)
    {
        model ??= new AppointmentViewModel { AppointmentDate = DateTime.Now.AddHours(1) };
        model.Patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
        model.Doctors = await _context.Doctors.Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync();
        return model;
    }
}
