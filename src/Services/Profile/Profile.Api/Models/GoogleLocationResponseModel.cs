using System.Collections.Generic;

namespace Profile.Api.Models
{
	public class GoogleLocationResponseModel
	{
		public List<Result> Results { get; set; }
		public string Status { get; set; }

	}

	public class Result
	{
		public string formatted_address { get; set; }
		public Geometry Geometry { get; set; }
	}

	public class Geometry
	{
		public Location Location { get; set; }
	}

	public class Location
	{
		public double Lat { get; set; }
		public double Lng { get; set; }
	}
}
