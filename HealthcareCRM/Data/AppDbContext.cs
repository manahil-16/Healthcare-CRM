using Microsoft.EntityFrameworkCore;

namespace HealthcareCRM.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
    }
}