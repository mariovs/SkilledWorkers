using System.Collections.Generic;

namespace Web.SkilledWorkers.HttpAggregator.Models
{
	public class UserProfileWithSkills : UserProfileInfo
	{
		public List<Skills> Skills { get; set; } = new List<Skills>();
	}
}
