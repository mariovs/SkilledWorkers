using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.SkilledWorkers.HttpAggregator.Config;
using Web.SkilledWorkers.HttpAggregator.Infrastructure;
using Web.SkilledWorkers.HttpAggregator.Models;
using static Web.SkilledWorkers.HttpAggregator.Config.ServicesUrlsConfig;

namespace Web.SkilledWorkers.HttpAggregator.Services
{
	public class SkillsService : ISkillsService
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly ServicesUrlsConfig _servicesUrlsConfig;
		private readonly ILogger<SkillsService> _logger;
		private readonly HttpServicesInteraction _httpServiceInteractor;

		public SkillsService(IHttpClientFactory clientFactory, IOptions<ServicesUrlsConfig> servicesUrlsConfig, ILogger<SkillsService> logger)
		{
			_logger = logger;
			_clientFactory = clientFactory;
			_servicesUrlsConfig = servicesUrlsConfig.Value;
			_httpServiceInteractor = new HttpServicesInteraction(_clientFactory, logger);
		}

		public async Task<UserSkillsInfo> GetUserSkills(string userId)
		{
			var url = $"{_servicesUrlsConfig.SkillsApi}{SkillsOperations.GetSkillsForUser(userId)}";
			return await _httpServiceInteractor.Get<UserSkillsInfo>(url);
		}
	}
}
