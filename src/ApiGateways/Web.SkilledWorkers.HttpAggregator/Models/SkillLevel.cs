﻿using System.ComponentModel.DataAnnotations;

namespace Web.SkilledWorkers.HttpAggregator.Models
{
	public class SkillLevel
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
