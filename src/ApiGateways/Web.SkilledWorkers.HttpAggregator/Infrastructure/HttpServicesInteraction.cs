using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.Support;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Web.SkilledWorkers.HttpAggregator.Infrastructure
{
	public class HttpServicesInteraction
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public HttpServicesInteraction(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<T> Get<T>(string url) where T : class
		{
			T result = null;
			var httpClient = _httpClientFactory.CreateClient(url);
			var response = await httpClient.GetAsync(new Uri(url));

			await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
			{
				if (x.IsFaulted)
					throw x.Exception;

				result = JsonConvert.DeserializeObject<T>(x.Result);
			});

			return result;
		}

		public async Task<List<T>> GetAllResultsIfPaginatedResult<T>(string url) where T : class
		{
			var resultsFound = new List<T>();
			var httpClient = _httpClientFactory.CreateClient(url);

			int currentPage = 0;
			var paramDictionary = new Dictionary<string, string>() {
					{ "pageNumber", currentPage.ToString()}
			};
			string getApiUrl = new Uri(QueryHelpers.AddQueryString(url, paramDictionary)).ToString();

			do
			{
				await httpClient.GetAsync(getApiUrl).ContinueWith(async (asyncGetTask) =>
				{
					var response = await asyncGetTask;
					if (response.IsSuccessStatusCode)
					{
						var deviceResponeObject = JsonConvert.DeserializeObject<PaginatedItems<T>>(await response.Content.ReadAsStringAsync());
						if (deviceResponeObject != null)
						{
							resultsFound.AddRange(deviceResponeObject.Data);

							currentPage = Convert.ToInt32(deviceResponeObject.PageIndex);
							var totalPages = (int)Math.Ceiling((double)deviceResponeObject.Count / deviceResponeObject.PageSize);
							if (currentPage < totalPages)
							{
								paramDictionary["page"] = (currentPage + 1).ToString();
								getApiUrl = new Uri(QueryHelpers.AddQueryString(url, paramDictionary)).ToString();
							}
							else
							{
								getApiUrl = string.Empty;
							}
						}
					}
					else
					{
						throw new QuerryAllFailedException();
					}
				});
			} while (!string.IsNullOrEmpty(getApiUrl));

			return resultsFound;
		}

		public async Task<T> PostRequest<T, TRequest>(string apiUrl, TRequest postObject) where T : class
		{
			T result = null;

			var httpClient = _httpClientFactory.CreateClient(apiUrl);
			var response = await httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json")).ConfigureAwait(false);

			response.EnsureSuccessStatusCode();

			await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
			{
				if (x.IsFaulted)
					throw x.Exception;

				result = JsonConvert.DeserializeObject<T>(x.Result);

			});

			return result;
		}

		public async Task<T> PutRequest<T>(string apiUrl, T putObject) where T : class
		{
			T result = null;
			var httpClient = _httpClientFactory.CreateClient(apiUrl);
			var response = await httpClient.PutAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(putObject), Encoding.UTF8, "application/json")).ConfigureAwait(false);

			response.EnsureSuccessStatusCode();

			await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
			{
				if (x.IsFaulted)
					throw x.Exception;

				result = JsonConvert.DeserializeObject<T>(x.Result);
			});

			return result;
		}

		public async Task<T> Delete<T>(string url) where T : class
		{
			T result = null;
			var httpClient = _httpClientFactory.CreateClient(url);
			var response = await httpClient.DeleteAsync(new Uri(url));

			await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
			{
				if (x.IsFaulted)
					throw x.Exception;

				result = JsonConvert.DeserializeObject<T>(x.Result);
			});

			return result;
		}
	}
}
