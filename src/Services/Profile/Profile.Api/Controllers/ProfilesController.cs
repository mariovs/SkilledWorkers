using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profile.Api.DataAccess;
using Profile.Api.Infrastructure;
using Profile.Api.Location;
using Profile.Api.Models;

namespace Profile.Api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProfilesController : ControllerBase
	{
		private readonly ILogger<ProfilesController> _logger;
		private readonly ProfileContext _profileContext;
		private readonly ILocationService _locationService;
		private readonly int maxProfileResultsPerPage = 50;
		private readonly IMapper _mapper;

		public ProfilesController(ILogger<ProfilesController> logger, ProfileContext profileContext, ILocationService locationService, IMapper mapper)
		{
			_logger = logger;
			_profileContext = profileContext;
			_locationService = locationService;
			_mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult<Models.Profile>> CreateProfile(ProfileRequestModel profileRequestModel)
		{
			var existingUser = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.UserId == profileRequestModel.UserId);
			if (existingUser != null)
			{
				return Forbid();
			}

			var locationResponse = await _locationService.GetCoordinates(profileRequestModel.GetFullAddress());
			if (locationResponse == null)
			{
				return BadRequest();
			}

			var profileEntity = _mapper.Map<Models.Profile>(profileRequestModel);
			profileEntity.Lat = locationResponse.Latitude;
			profileEntity.Long = locationResponse.Longitude;

			var profileResponse = _profileContext.Profiles.Add(profileEntity);
			await _profileContext.SaveChangesAsync();

			return profileResponse.Entity;
		}

		[HttpGet]
		[Route("{userId}")]
		public async Task<ActionResult<Models.Profile>> GetProfile(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return BadRequest();
			}

			var profile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.UserId.Equals(userId));

			if (profile != null)
			{
				return profile;
			}
			return NotFound();
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedItems<Models.Profile>>> GetAllProfiles(int pageSize = 10, int pageNumber = 0)
		{
			if (pageSize > maxProfileResultsPerPage || pageNumber < 0)
			{
				return BadRequest();
			}

			var profilesCount = _profileContext.Profiles.Count();
			if (profilesCount == 0)
			{
				new PaginatedItems<Models.Profile>(pageNumber, pageSize, profilesCount, null);
			}

			var profiles = await _profileContext.Profiles
									.Skip(pageSize * pageNumber)
									.Take(pageSize)
									.ToListAsync();

			return new PaginatedItems<Models.Profile>(pageNumber, pageSize, profilesCount, profiles);
		}

		[HttpPut]
		public async Task<ActionResult<Models.Profile>> UpdateProfile(ProfileRequestModel profileRequestModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			//check if userid from auth or is admin

			var locationResponse = await _locationService.GetCoordinates(profileRequestModel.GetFullAddress());
			if (locationResponse == null)
			{
				return BadRequest();
			}

			var existingProfile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.UserId.Equals(profileRequestModel.UserId));
			if (existingProfile == null)
			{
				return NotFound();
			}

			var updatedProfile = _mapper.Map<Models.Profile>(profileRequestModel);
			updatedProfile.Lat = locationResponse.Latitude;
			updatedProfile.Long = locationResponse.Longitude;

			_profileContext.Entry(existingProfile).CurrentValues.SetValues(updatedProfile);
			await _profileContext.SaveChangesAsync();

			return existingProfile;
		}

		[HttpDelete]
		[Route("{userId}")]
		public async Task<ActionResult<bool>> DeleteProfile(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return BadRequest();
			}
			//verify token or admin role

			try
			{
				var existingProfile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.UserId == userId);
				if (existingProfile == null)
				{
					return NotFound();
				}

				_profileContext.Profiles.Remove(existingProfile);
				await _profileContext.SaveChangesAsync();
				return true;
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error during deletion of user: {userId}");
				return false;
			}
		}

		[HttpGet]
		[Route("search")]
		public async Task<ActionResult<PaginatedItems<Models.Profile>>> GetProfilesByAddress(string streetName, string city, double radius, int pageSize = 10, int pageNumber = 0)
		{
			if (pageSize > maxProfileResultsPerPage || pageNumber < 0 ||
				string.IsNullOrWhiteSpace(streetName) || string.IsNullOrWhiteSpace(city))
			{
				return BadRequest();
			}

			var baseAddressCoordinates = await _locationService.GetCoordinates($"{streetName} {city}");
			if (baseAddressCoordinates == null)
			{
				return BadRequest();
			}

			//todo: replace it with sql code to increase performance, entity framework doesn't know about Math library
			//this will retrive all profiles from the city and will execute the search on the server
			var searchQuery = _profileContext.Profiles
									.Where(p => p.City == city)
									.Where(x => (12742 * Math.Asin(Math.Sqrt(Math.Sin(((Math.PI / 180) * (x.Lat - baseAddressCoordinates.Latitude)) / 2) * Math.Sin(((Math.PI / 180) * (x.Lat - baseAddressCoordinates.Latitude)) / 2) +
										Math.Cos((Math.PI / 180) * baseAddressCoordinates.Latitude) * Math.Cos((Math.PI / 180) * (x.Lat)) *
										Math.Sin(((Math.PI / 180) * (x.Long - baseAddressCoordinates.Longitude)) / 2) * Math.Sin(((Math.PI / 180) * (x.Long - baseAddressCoordinates.Longitude)) / 2)))) <= radius);

			_profileContext.Profiles.OrderBy(x => x.City);

			var totalCount = searchQuery.AsNoTracking().Count();
			if (totalCount == 0)
			{
				new PaginatedItems<Models.Profile>(pageNumber, pageSize, totalCount, null);
			}

			var profilesFound = await searchQuery.Skip(pageSize * pageNumber)
									.Take(pageSize)
									.AsNoTracking()
									.ToListAsync();

			return new PaginatedItems<Models.Profile>(pageNumber, pageSize, totalCount, profilesFound);
		}
	}
}