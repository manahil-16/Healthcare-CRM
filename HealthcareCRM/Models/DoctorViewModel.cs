using System.ComponentModel.DataAnnotations;

namespace HealthcareCRM.Models
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Schedule days are required")]
        [StringLength(100)]
        [Display(Name = "Schedule days")]
        public string ScheduleDays { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
