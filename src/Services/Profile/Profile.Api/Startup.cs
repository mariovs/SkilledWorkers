using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Profile.Api.Config;
using Profile.Api.DataAccess;
using Profile.Api.Infrastructure;
using Profile.Api.Location;

namespace Profile.Api
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
			services.AddDbContext<ProfileContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("Default"));
			});

			services.AddCors();
			services.AddControllers(options =>
			{
				options.Filters.Add(typeof(ValidatorActionFilter));
			});

			services.Configure<LocationConfig>(Configuration.GetSection(nameof(LocationConfig)));
			var locationConfig = Configuration.GetSection(nameof(LocationConfig)).Get<LocationConfig>();
			services.AddHttpClient("googleMaps", c =>
			{
				c.BaseAddress = new Uri(locationConfig.ApiUrl);
			})
			.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
			{
				TimeSpan.FromSeconds(1),
				TimeSpan.FromSeconds(5),
				TimeSpan.FromSeconds(10)
			}));

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
				options.AddPolicy("Professional", policy => policy.RequireRole("Professional"));
			});

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.Authority = "https://dev-ujfz2lay.eu.auth0.com/";
				options.Audience = "https://profile.skilledworkers.com/api";
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = false,
					//IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("WBibFyxYndf87KwTp31YbXUZR1DU8JTYZsEmLRIN8Izrq4AMo3-9vdClc7aDBFW3")),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			services.AddAutoMapper(config =>
			{
				config.AddProfile<MappingProfile>();
			}, typeof(Startup));

			services.AddTransient<ILocationService, GoogleLocationService>();
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

			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
