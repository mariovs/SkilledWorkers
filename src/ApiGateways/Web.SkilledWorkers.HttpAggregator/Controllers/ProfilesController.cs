using System.Threading.Tasks;
using Api.Support;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
			var userProfileInfo = await _profileService.GetUserById(userId);
			if (userProfileInfo == null)
			{
				return NotFound();
			}
			var userWithSkills = _mapper.Map<UserProfileWithSkills>(userProfileInfo);

			var userSkills = await _skillsService.GetUserSkills(userId);
			if (userSkills != null)
			{
				userWithSkills.Skills = userSkills.Skills;
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