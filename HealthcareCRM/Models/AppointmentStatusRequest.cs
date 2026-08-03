using System.ComponentModel.DataAnnotations;

namespace HealthcareCRM.Models;

public class AppointmentStatusRequest
{
    [Required]
    [RegularExpression("^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Status must be Pending, Confirmed, or Cancelled.")]
    public string Status { get; set; } = string.Empty;
}
