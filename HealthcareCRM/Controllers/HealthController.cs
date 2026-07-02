using Microsoft.AspNetCore.Mvc;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        // Returns API health status
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new 
            { 
                success = true, 
                message = "HealthcareCRM API is running",
                data = new { status = "Healthy", timestamp = DateTime.UtcNow }
            });
        }
    }
}