using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Support;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Profile.Api.Controllers;
using Profile.Api.DataAccess;
using Profile.Api.Infrastructure;
using Profile.Api.Location;
using Profile.Api.Models;
using Xunit;

namespace Profile.UnitTests
{
	public class ProfileControllerTests
	{
		private readonly DbContextOptions<ProfileContext> _dbOptions;
		private readonly IMapper _mapper;

		public ProfileControllerTests()
		{
			_dbOptions = new DbContextOptionsBuilder<ProfileContext>()
			.UseInMemoryDatabase(databaseName: "in-memory")
			.Options;

			var config = new MapperConfiguration(opts =>
			{
				opts.AddProfile<MappingProfile>();
				
			});
			_mapper = config.CreateMapper();
			using (var dbContext = new ProfileContext(_dbOptions))
			{
				if (dbContext.Profiles.Count() == 0)
				{
					dbContext.Profiles.AddRange(GetFakeProfiles());
					dbContext.SaveChanges();
				}
			}
		}

		[Fact]
		public async Task Get_Profile_Success()
		{
			//Arrange
			var userId = "testUser1";
			var profileController = SetupProfilesController();

			//Act
			var actionResult = await profileController.GetProfile(userId);

			//Assert
			Assert.IsType<ActionResult<Api.Models.Profile>>(actionResult);
			Assert.Equal(actionResult.Value.FirstName, "TestUserFirstName1");
			Assert.Equal(actionResult.Value.LastName, "TestUserLastName1");
		}

		[Fact]
		public async Task Get_All_Profiles_Success()
		{
			var profileControler = SetupProfilesController();

			var actionResult = await profileControler.GetAllProfiles();

			Assert.IsType<ActionResult<PaginatedItems<Api.Models.Profile>>>(actionResult);
			Assert.True(actionResult.Value.Count == 4);
			Assert.Equal(4, actionResult.Value.Data.Count());
		}

		[Fact]
		public async Task Search_For_City_Works()
		{
			var profilesController = SetupProfilesController();

			var actionResultSFProfiles = await profilesController.GetProfilesByAddress("Union Street", 2);
			var actionResultZurichProfilesShortDistance = await profilesController.GetProfilesByAddress("Bahnhofstrasse", 1);
			var actionResultZurichProfilesLongDistance = await profilesController.GetProfilesByAddress("Bahnhofstrasse", int.MaxValue);

			Assert.Equal(2, actionResultSFProfiles.Value.Count);
			Assert.Equal(1, actionResultZurichProfilesShortDistance.Value.Count);
			Assert.Equal(2, actionResultZurichProfilesLongDistance.Value.Count);
		}

		private ProfilesController SetupProfilesController()
		{
			var mockLogger = new Mock<ILogger<ProfilesController>>().Object;
			var mockedLocationService = new Mock<ILocationService>();
			mockedLocationService.Setup(s => s.GetCoordinates("Union Street San Francisco"))
				.ReturnsAsync(new AddressCoordinates()
				{
					Latitude = 10,
					Longitude = 10
				});

			mockedLocationService.Setup(s => s.GetCoordinates("Bahnhofstrasse Zurich"))
				.ReturnsAsync(new AddressCoordinates()
				{
					Latitude = 40,
					Longitude = 40
				});

			//mockedLocationService.Setup(s => s.GetCoordinates("Bauhallengasse Zurich"))
			//	.ReturnsAsync(new AddressCoordinates()
			//	{
			//		Latitude = 60,
			//		Longitude = 60
			//	});

			var profileContext = new ProfileContext(_dbOptions);
			return new ProfilesController(mockLogger, profileContext, mockedLocationService.Object, _mapper);
		}

		private List<Api.Models.Profile> GetFakeProfiles()
		{
			return new List<Api.Models.Profile>()
			{
				new Api.Models.Profile()
				{
						UserId = "testUser1",
						FirstName = "TestUserFirstName1",
						LastName = "TestUserLastName1",
						City = "San Francisco",
						StreetName = "Union Street",
						Country = "USA",
						State = "California",
						ZipCode = "55500",
						Lat = 10,
						Long = 10,
				},
								new Api.Models.Profile()
				{
						UserId = "testUser2",
						FirstName = "TestUserFirstName2",
						LastName = "TestUserLastName2",
						City = "San Francisco",
						StreetName = "Union Street",
						Country = "USA",
						State = "California",
						ZipCode = "55500",
						Lat = 10.01,
						Long = 10.0,
				},
				new Api.Models.Profile()
				{
						UserId = "testUser3",
						FirstName = "TestUserFirstName3",
						LastName = "TestUserLastName3",
						City = "Zurich",
						StreetName = "Bahnhofstrasse",
						Country = "Switzerland",
						ZipCode = "8001",
						Lat = 40,
						Long = 40,
				},
				new Api.Models.Profile()
				{
						UserId = "testUser4",
						FirstName = "TestUserFirstName4",
						LastName = "TestUserLastName4",
						City = "Zurich",
						StreetName = "Bauhallengasse",
						Country = "Switzerland",
						ZipCode = "8003",
						Lat = 60,
						Long = 60,
				}
			};
		}
	}
}
