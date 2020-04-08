using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.SkilledWorkers.HttpAggregator.Config;
using Web.SkilledWorkers.HttpAggregator.Infrastructure;
using Web.SkilledWorkers.HttpAggregator.Services;

namespace Web.SkilledWorkers.HttpAggregator
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				options.Filters.Add(typeof(ValidatorActionFilter));
			});

			services.Configure<ServicesUrlsConfig>(Configuration.GetSection(nameof(ServicesUrlsConfig)));
			var serviceUrlsConfig = Configuration.GetSection(nameof(ServicesUrlsConfig)).Get<ServicesUrlsConfig>();

			//setup retries for services 
			services.AddHttpClientForService("profileService", serviceUrlsConfig.ProfilesApi);
			services.AddHttpClientForService("skillsService", serviceUrlsConfig.SkillsApi);

			services.AddAutoMapper(config =>
			{
				config.AddProfile<MappingProfile>();
			}, typeof(Startup));

			services.AddTransient<IProfileService, ProfileService>();
			services.AddTransient<ISkillsService, SkillsService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
