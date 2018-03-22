using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DutchTreat
{
  public class Startup
  {
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<StoreUser, IdentityRole>(cfg =>
			{
				cfg.User.RequireUniqueEmail = true;
			})
				.AddEntityFrameworkStores<DutchContext>(); //supporting auth based on cookies

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                });

			services.AddDbContext<DutchContext>( cfg => {
				cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
			});

			services.AddAutoMapper();

			services.AddTransient<IMailService, NullMailService>();
			services.AddTransient<DutchSeeder>();

			//Add IDutchRepository that people can use and use DutchRepository implementation of it.
			services.AddScoped<IDutchRepository, DutchRepository>();
			/*in testing project...you can mock it or use another.
			services.AddScoped<IDutchRepository, MockDutchRepository>();
			*/

			//Support for real mail service
			services.AddMvc()
				.AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(cfg =>
			{
				cfg.MapRoute("Default", "{controller}/{action}/{id?}", new { Controller = "App", Action = "Index" });
			});

			if (env.IsDevelopment())
			{
				//Seed the db
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
					seeder.Seed().Wait();
				}
			}

		}
	}
}
