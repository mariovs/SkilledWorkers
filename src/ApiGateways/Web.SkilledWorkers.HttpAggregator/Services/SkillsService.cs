using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Support;
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

		public async Task<UserSkillsInfo> GetUserWithSkills(string userId, string professionName, string skillLvlName)
		{
			var url = $"{_servicesUrlsConfig.SkillsApi}{SkillsOperations.GetSkillsForUser(userId, professionName, skillLvlName)}";
			return await _httpServiceInteractor.Get<UserSkillsInfo>(url);
		}

		public async Task<PaginatedItems<UserSkillsInfo>> GetUsersSkills(List<string> userIdList, string professionName, string skillLvlName, int pageNumber, int pageSize)
		{
			var url =  SkillsOperations.GetSkillsForUserIds(_servicesUrlsConfig.SkillsApi, userIdList.ToArray(), professionName, skillLvlName, pageSize, pageNumber);
			return await _httpServiceInteractor.Get<PaginatedItems<UserSkillsInfo>>(url);
		}
	}
}
