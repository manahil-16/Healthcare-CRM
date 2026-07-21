using System.ComponentModel.DataAnnotations;

namespace HealthcareCRM.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Patient is required")]
        public int PatientId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Doctor is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Date and time is required")]
        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = "Pending";

        public string Notes { get; set; } = string.Empty;

        // For dropdowns
        public List<Patient> Patients { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();

        // For display
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
