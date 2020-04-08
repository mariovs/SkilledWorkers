using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.SkilledWorkers.HttpAggregator.Models;
using Web.SkilledWorkers.HttpAggregator.Services;

namespace Web.SkilledWorkers.HttpAggregator.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IProfileService _profileService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IProfileService profileService)
		{
			_logger = logger;
			_profileService = profileService;
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<UserProfileInfo>> Get(string userId)
		{
			return await _profileService.GetUserById(userId);
		}
	}
}
