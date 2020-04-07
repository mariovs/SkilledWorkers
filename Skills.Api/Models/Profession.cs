using System.ComponentModel.DataAnnotations;

namespace Skills.Api.Models
{
	public class Profession
	{
		[Key]
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(250)]
		public string Description { get; set; }
	}
}
