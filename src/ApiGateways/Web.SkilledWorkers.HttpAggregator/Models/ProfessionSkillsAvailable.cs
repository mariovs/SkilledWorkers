using System.ComponentModel.DataAnnotations;

namespace Web.SkilledWorkers.HttpAggregator.Models
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
