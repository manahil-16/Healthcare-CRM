using System.ComponentModel.DataAnnotations;

namespace HealthcareCRM.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
    }
}