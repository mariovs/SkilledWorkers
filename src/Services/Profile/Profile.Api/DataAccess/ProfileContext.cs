using Microsoft.EntityFrameworkCore;

namespace Profile.Api.DataAccess
{
	public class ProfileContext : DbContext
	{
		public ProfileContext(DbContextOptions options) : base(options) { }

		public DbSet<Models.Profile> Profiles { get; set; }
	}
}
