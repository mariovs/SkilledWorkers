using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Profile.Api.DataAccess;

namespace Profile.Api.Infrastructure
{
	public static class SeedDb
	{
		public static void InsertFakeData(ProfileContext dbContext)
		{
			if (dbContext.Profiles.Count() == 0)
			{
				dbContext.Profiles.AddRange(GetFakeProfiles());
				dbContext.SaveChanges();
			}
		}


		private static List<Api.Models.Profile> GetFakeProfiles()
		{
			return new List<Api.Models.Profile>()
			{
				new Api.Models.Profile()
				{
						UserId = "testUser1",
						FirstName = "Benjamin",
						LastName = "Franklin",
						City = "Washington",
						StreetName = "1600 Pennsylvania Ave NW",
						Country = "USA",
						ZipCode = "20500",
						Lat = 38.900770,
						Long = -77.026430,
				},
				new Api.Models.Profile()
				{
						UserId = "testUser2",
						FirstName = "Theodore",
						LastName = "Roosevelt",
						City = "Washington",
						StreetName = "1600 Pennsylvania Ave NW",
						Country = "USA",
						ZipCode = "20500",
						Lat = 38.898770,
						Long = -77.036430,
				},
				new Api.Models.Profile()
				{
						UserId = "testUser3",
						FirstName = "Abraham",
						LastName = "Lincoln",
						City = "Washington",
						StreetName = "1600 Pennsylvania Ave NW",
						Country = "USA",
						ZipCode = "20500",
						Lat = 38.898770,
						Long = -77.036430,
				},
				new Api.Models.Profile()
				{
						UserId = "testUser4",
						FirstName = "Mario",
						LastName = "Vasile",
						City = "Zurich",
						StreetName = "Bahnhoffstrasse",
						Country = "Switzerland",
						ZipCode = "8001",
						Lat = 47.371550,
						Long = 8.538590,
				}
			};
		}
	}
}
