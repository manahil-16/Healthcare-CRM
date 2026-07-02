using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Models;

namespace HealthcareCRM.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}