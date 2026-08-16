using HealthcareCRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Database.CanConnect())
        {
            context.Database.EnsureCreated();
        }

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                FullName = "System Administrator",
                Email = "admin@healthcarecrm.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            context.Users.Add(new User
            {
                FullName = "Nadia Sheikh",
                Email = "staff@healthcarecrm.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                Role = "Staff",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!context.Doctors.Any())
        {
            context.Doctors.AddRange(
                new Doctor { Name = "Dr. Ayesha Khan", Specialization = "Cardiology", Phone = "0300-1112233", IsActive = true, ScheduleDays = "Mon,Wed,Fri" },
                new Doctor { Name = "Dr. Bilal Ahmed", Specialization = "General Medicine", Phone = "0300-2223344", IsActive = true, ScheduleDays = "Tue,Thu" },
                new Doctor { Name = "Dr. Sara Malik", Specialization = "Pediatrics", Phone = "0300-3334455", IsActive = true, ScheduleDays = "Mon,Tue,Thu,Fri" },
                new Doctor { Name = "Dr. Hamza Ali", Specialization = "Orthopedics", Phone = "0300-4445566", IsActive = true, ScheduleDays = "Wed,Fri" }
            );
        }

        context.SaveChanges();

        if (!context.Patients.Any())
        {
            context.Patients.AddRange(
                new Patient { FullName = "Ali Hassan", Email = "ali.hassan@example.com", Phone = "0333-1001001", Address = "G-8, Islamabad", DateOfBirth = new DateTime(1992, 5, 14) },
                new Patient { FullName = "Maria Noor", Email = "maria.noor@example.com", Phone = "0333-1001002", Address = "F-6, Islamabad", DateOfBirth = new DateTime(1988, 9, 30) },
                new Patient { FullName = "Usman Tariq", Email = "usman.tariq@example.com", Phone = "0333-1001003", Address = "I-8, Islamabad", DateOfBirth = new DateTime(1995, 1, 9) },
                new Patient { FullName = "Areeba Qureshi", Email = "areeba.qureshi@example.com", Phone = "0333-1001004", Address = "Blue Area, Islamabad", DateOfBirth = new DateTime(2001, 7, 21) },
                new Patient { FullName = "Hassan Raza", Email = "hassan.raza@example.com", Phone = "0333-1001005", Address = "Rawalpindi", DateOfBirth = new DateTime(1985, 12, 2) },
                new Patient { FullName = "Nadia Iqbal", Email = "nadia.iqbal@example.com", Phone = "0333-1001006", Address = "Sihala, Islamabad", DateOfBirth = new DateTime(1991, 4, 18) }
            );
        }

        context.SaveChanges();

        if (!context.Appointments.Any())
        {
            var patients = context.Patients.OrderBy(p => p.Id).ToList();
            var doctors = context.Doctors.OrderBy(d => d.Id).ToList();

            context.Appointments.AddRange(
                new Appointment { PatientId = patients[0].Id, DoctorId = doctors[0].Id, AppointmentDate = DateTime.Now.AddDays(1), Status = "Pending", Notes = "Follow-up checkup" },
                new Appointment { PatientId = patients[1].Id, DoctorId = doctors[1].Id, AppointmentDate = DateTime.Now.AddDays(2), Status = "Confirmed", Notes = "Cardiology consultation" },
                new Appointment { PatientId = patients[2].Id, DoctorId = doctors[2].Id, AppointmentDate = DateTime.Now.AddDays(3), Status = "Cancelled", Notes = "Pediatric review" },
                new Appointment { PatientId = patients[3].Id, DoctorId = doctors[3].Id, AppointmentDate = DateTime.Now.AddDays(4), Status = "Pending", Notes = "Orthopedic screening" },
                new Appointment { PatientId = patients[4].Id, DoctorId = doctors[0].Id, AppointmentDate = DateTime.Now.AddDays(5), Status = "Confirmed", Notes = "Heart monitoring" }
            );
        }

        context.SaveChanges();
    }
}
