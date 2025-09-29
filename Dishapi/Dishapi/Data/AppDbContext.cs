using Microsoft.EntityFrameworkCore;
using Dishapi.Models;

namespace Dishapi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

		public DbSet<Profile> Profiles { get; set; } = null!;
		// Add other DbSets (Dishes, etc) as needed
	}
}
