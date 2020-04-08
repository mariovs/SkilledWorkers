using System.ComponentModel.DataAnnotations;

namespace Skills.Api.Models
{
	public class ProfessionSkillsAvailable
	{
		[Required]
		[MaxLength(50)]
		public string ProfessionName { get; set; }

		[Required]
		[MaxLength(50)]
		public string SkillLevelName { get; set; }
	}
}
