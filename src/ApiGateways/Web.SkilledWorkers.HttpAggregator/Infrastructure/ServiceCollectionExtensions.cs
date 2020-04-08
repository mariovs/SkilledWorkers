using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Web.SkilledWorkers.HttpAggregator.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static void AddHttpClientForService(this IServiceCollection services, string serviceName, string serviceUrl)
		{
			services.AddHttpClient(serviceName, c =>
			{
				c.BaseAddress = new Uri(serviceUrl);
			})
			.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
			{
				TimeSpan.FromSeconds(1),
				TimeSpan.FromSeconds(5),
				TimeSpan.FromSeconds(10)
			}));
		}
	}
}
