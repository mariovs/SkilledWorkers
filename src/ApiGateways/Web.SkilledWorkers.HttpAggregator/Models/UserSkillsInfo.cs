using System.Collections.Generic;

namespace Web.SkilledWorkers.HttpAggregator.Models
{
	public class UserSkillsInfo
	{
		public string UserId { get; set; }

		public List<Skills> Skills { get; set; } = new List<Skills>();
	}
}
