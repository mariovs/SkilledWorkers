using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
			services.AddCors();
			services.AddControllers(options =>
			{
				options.Filters.Add(typeof(ValidatorActionFilter));
				options.Filters.Add(new ServicesExceptionFilter());
			});

			services.Configure<ServicesUrlsConfig>(Configuration.GetSection(nameof(ServicesUrlsConfig)));
			var serviceUrlsConfig = Configuration.GetSection(nameof(ServicesUrlsConfig)).Get<ServicesUrlsConfig>();

			//setup retries for services 
			services.AddHttpClientForService("profileService", serviceUrlsConfig.ProfilesApi);
			services.AddHttpClientForService("skillsService", serviceUrlsConfig.SkillsApi);

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skilled Workers API", Version = "v1" });

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

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

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skilled Workers API");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseAuthorization();
			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
