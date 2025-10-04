using Dishapi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Class1> Classes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        // Removed: public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}