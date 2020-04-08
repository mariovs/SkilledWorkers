using System.Collections.Generic;

namespace Skills.Api.Models
{
	public class ProfessionSkillAvailableResponse
	{
		public Profession Profession { get; set; }

		public List<SkillLevel> SkillLevelsAvailable { get; set; } = new List<SkillLevel>();
	}
}
