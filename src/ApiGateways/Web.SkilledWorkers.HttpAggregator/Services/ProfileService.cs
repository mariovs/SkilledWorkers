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
	public class ProfileService : IProfileService
	{
		private readonly ILogger<ProfileService> _logger;
		private readonly IHttpClientFactory _clientFactory;
		private readonly ServicesUrlsConfig _servicesUrlsConfig;
		private readonly HttpServicesInteraction _httpServiceInteractor;

		public ProfileService(IHttpClientFactory clientFactory, IOptions<ServicesUrlsConfig> servicesUrlsConfig, ILogger<ProfileService> logger)
		{
			_clientFactory = clientFactory;
			_servicesUrlsConfig = servicesUrlsConfig.Value;
			_logger = logger;
			_httpServiceInteractor = new HttpServicesInteraction(_clientFactory, logger);
		}

		public async Task<UserProfileInfo> CreateUser(UserProfileInfoRequest userProfileRequest)
		{
			var url = $"{_servicesUrlsConfig.ProfilesApi}{ProfileOperations.CreateProfile()}";

			return await _httpServiceInteractor.PostRequest<UserProfileInfo, UserProfileInfoRequest>(url, userProfileRequest);
		}

		public async Task<UserProfileInfo> GetUserById(string userId)
		{
			var url = $"{_servicesUrlsConfig.ProfilesApi}{ProfileOperations.GetProfile(userId)}";
			return await _httpServiceInteractor.Get<UserProfileInfo>(url);
		}

		public async Task<PaginatedItems<UserProfileInfo>> GetUserProfileInfoList(int pageNumber, int pageSize)
		{
			var url = $"{_servicesUrlsConfig.ProfilesApi}{ProfileOperations.GetProfiles(pageSize, pageNumber)}";
			return await _httpServiceInteractor.Get<PaginatedItems<UserProfileInfo>>(url);
		}

		public async Task<PaginatedItems<UserProfileInfo>> GetUserProfileInfoList(string address, double radius, int pageNumber = 0, int pageSize = 10)
		{
			var url = $"{_servicesUrlsConfig.ProfilesApi}{ProfileOperations.GetProfiles(address, radius, pageSize, pageNumber)}";
			return await _httpServiceInteractor.Get<PaginatedItems<UserProfileInfo>>(url);
		}
	}
}
