using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Profile.Api.Config;
using Profile.Api.Models;

namespace Profile.Api.Location
{
	public class GoogleLocationService : ILocationService
	{

		private readonly IHttpClientFactory _clientFactory;
		private readonly ILogger _logger;
		private readonly LocationConfig _locationConfig;

		public GoogleLocationService(IHttpClientFactory clientFactory, IOptions<LocationConfig> locationConfig, ILogger<GoogleLocationService> logger)
		{
			_clientFactory = clientFactory;
			_logger = logger;
			_locationConfig = locationConfig.Value;
		}

		public async Task<AddressCoordinates> GetCoordinates(string address)
		{
			var paramDictionary = new Dictionary<string, string>() {
				{ "address", address},
				{ "key", _locationConfig.ApiKey}
			};
			var client = _clientFactory.CreateClient("googleMaps");
			var apiUrl = $"{client.BaseAddress}/maps/api/geocode/json";
			var apiResponse = await client.GetAsync(new Uri(QueryHelpers.AddQueryString(apiUrl, paramDictionary)).ToString());
			apiResponse.EnsureSuccessStatusCode();

			var contentString = await apiResponse.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<GoogleLocationResponseModel>(contentString);

			if (responseModel.Status.Equals("ZERO_RESULTS", StringComparison.InvariantCultureIgnoreCase) ||
			   responseModel.Status.Equals("INVALID_REQUEST", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			if (!responseModel.Status.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
			{
				var errorMessage = $"Location service failed with status { responseModel.Status} for input : {address}";
				_logger.LogError(errorMessage);
				throw new Exception(errorMessage);
			}

			var locationResult = responseModel.Results.FirstOrDefault();
			if (locationResult == null)
			{
				return null;
			}

			return new AddressCoordinates()
			{
				Latitude = locationResult.Geometry.Location.Lat,
				Longitude = locationResult.Geometry.Location.Lng
			};

		}
	}
}
