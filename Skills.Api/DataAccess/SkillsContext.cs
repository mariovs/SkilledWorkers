using Microsoft.EntityFrameworkCore;
using Skills.Api.Models;

namespace Skills.Api.DataAccess
{
	public class SkillsContext : DbContext
	{
		public SkillsContext(DbContextOptions options) : base(options) { }

		public DbSet<UserSkills> UserSkills { get; set; }

		public DbSet<Models.Skills> Skills { get; set; }

		public DbSet<Profession> Professions { get; set; }

		public DbSet<SkillLevel> SkillLevels { get; set; }

		public DbSet<ProfessionSkillsAvailable> ProfessionSkillAvailable { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProfessionSkillsAvailable>()
				.HasKey(ps => new { ps.ProfessionName, ps.SkillLevelName });
		}
	}
}
