using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Skills.Api.Models
{
	public class UserSkills
	{
		[Key]
		public string UserId { get; set; }

		public List<Skills> Skills { get; set; } = new List<Skills>();
	}
}
