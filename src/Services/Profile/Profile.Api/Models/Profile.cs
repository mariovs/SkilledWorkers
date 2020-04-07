using System.ComponentModel.DataAnnotations;

namespace Profile.Api.Models
{
	public class Profile
	{
		[Key]
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

		[Required]
		[MaxLength(100)]
		public string Country { get; set; }

		[MaxLength(100)]
		public string State { get; set; }

		[MaxLength(10)]
		public string ZipCode { get; set; }

		[Required]
		public double Lat { get; set; }

		[Required]
		public double Long { get; set; }
	}
}
