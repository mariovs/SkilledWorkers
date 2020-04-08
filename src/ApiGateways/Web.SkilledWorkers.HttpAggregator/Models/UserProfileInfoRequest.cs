using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.SkilledWorkers.HttpAggregator.Models
{
	public class UserProfileInfoRequest
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(300)]
		public string StreetName { get; set; }

		[Required]
		[MaxLength(100)]
		public string City { get; set; }

		[MaxLength(100)]
		public string State { get; set; }

		[MaxLength(100)]
		public string Country { get; set; }

		[MaxLength(10)]
		public string ZipCode { get; set; }
	}
}
