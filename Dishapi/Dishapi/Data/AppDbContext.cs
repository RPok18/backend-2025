using Microsoft.EntityFrameworkCore;
using Dishapi.Models;

namespace Dishapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; } = null!;

        
    }
}
