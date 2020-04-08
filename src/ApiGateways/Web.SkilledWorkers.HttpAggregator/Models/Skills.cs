using System;

namespace Web.SkilledWorkers.HttpAggregator.Models
{
	public class Skills
	{
		public Guid Id { get; set; }
		public Profession Profession { get; set; }
		public SkillLevel SkillLevel { get; set; }
	}
}
