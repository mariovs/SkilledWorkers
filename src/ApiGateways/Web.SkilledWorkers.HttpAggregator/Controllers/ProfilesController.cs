using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Support;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.SkilledWorkers.HttpAggregator.Infrastructure;
using Web.SkilledWorkers.HttpAggregator.Models;
using Web.SkilledWorkers.HttpAggregator.Services;

namespace Web.SkilledWorkers.HttpAggregator.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProfilesController : ControllerBase
	{
		private readonly ILogger<ProfilesController> _logger;
		private readonly IProfileService _profileService;
		private readonly ISkillsService _skillsService;
		private readonly IMapper _mapper;

		public ProfilesController(ILogger<ProfilesController> logger, IProfileService profileService, ISkillsService skillsService, IMapper mapper)
		{
			_logger = logger;
			_profileService = profileService;
			_skillsService = skillsService;
			_mapper = mapper;
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<UserProfileWithSkills>> Get(string userId)
		{
			var userProfileFound = await _profileService.GetUserById(userId);

			if (userProfileFound == null)
			{
				return NotFound();
			}

			var userWithSkills = _mapper.Map<UserProfileWithSkills>(userProfileFound);

			UserSkillsInfo userSkillsInfo = null;
			try
			{
				userSkillsInfo = await _skillsService.GetUserSkills(userId);
			}
			catch (HttpExceptionWithStatusCode ex)
			{
				//if user doesn't have skills it's ok
				if(ex.StatusCode != HttpStatusCode.NotFound)
				{
					throw;
				}
			}

			if (userSkillsInfo != null)
			{
				userWithSkills.Skills = userSkillsInfo.Skills;
			}

			return userWithSkills;
		}

		/// <summary>
		/// Get the list of users profile
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<PaginatedItems<UserProfileInfo>>> GetUserProfileInfoList(int pageNumber = 0, int pageSize = 10)
		{
			return await _profileService.GetUserProfileInfoList(pageNumber, pageSize);
		}

		[HttpPost]
		public async Task<ActionResult<UserProfileInfo>> CreateUserProfileInfo(UserProfileInfoRequest userInfo)
		{
			return await _profileService.CreateUser(userInfo);
		}


	}
}