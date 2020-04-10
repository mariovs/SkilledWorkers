using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Skills.Api.Config;
using Skills.Api.DataAccess;
using Skills.Api.Ifrastructure;
using Skills.Api.Infrastructure;

namespace Skills.Api
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

			var dbInMemory = Configuration.GetValue<bool>("InMemmoryDb");
			if (dbInMemory)
			{
				services.AddDbContext<SkillsContext>(options =>
				{
					options.UseInMemoryDatabase(databaseName: "in-memory");
				});
			}
			else
			{
				services.AddDbContext<SkillsContext>(options =>
				{

					options.UseSqlServer(Configuration.GetConnectionString("Default"));
				});
			}

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skills API", Version = "v1" });

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			services.Configure<ApiLimitsConfig>(Configuration.GetSection(nameof(ApiLimitsConfig)));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//insert test data in db
			var context = (SkillsContext)serviceProvider.GetService(typeof(SkillsContext));
			SeedData.PopulateWithData(context);

			app.UseHttpsRedirection();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseAuthorization();



			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
