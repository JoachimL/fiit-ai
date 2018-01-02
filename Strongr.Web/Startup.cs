using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Strongr.Web.Data;
using Strongr.Web.Models;
using Strongr.Web.Services;
using StructureMap;
using ElCamino.AspNetCore.Identity.AzureTable;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Identity;
using Strongr.Web.Validation;

namespace Strongr.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole>()
              .AddAzureTableStores<ApplicationDbContext>(BuildIdentityConfiguration)
              .AddDefaultTokenProviders()
              .CreateAzureTablesIfNotExists<ApplicationDbContext>(); //can remove after first run;

            services.AddAuthentication().AddFacebook(facebookOptions =>
                  {
                      facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                      facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                  });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCors();



            services.AddMvc(setup =>
            {
                setup.Filters.Add(new ValidationActionFilter());
            }).AddControllersAsServices();

            var container = new Container();
            container.Configure(x =>
            {
                x.AddRegistry<StrongrRegistry>();
                x.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        private IdentityConfiguration BuildIdentityConfiguration()
        {
            return new IdentityConfiguration
            {
                TablePrefix =
                Configuration
                  .GetSection("IdentityAzureTable:IdentityConfiguration:TablePrefix").Value,
                StorageConnectionString = Configuration.GetSection("IdentityAzureTable:IdentityConfiguration:StorageConnectionString").Value,
                LocationMode = Configuration.GetSection("IdentityAzureTable:IdentityConfiguration:LocationMode").Value
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:8080")
                 .AllowAnyMethod()
                 .AllowAnyHeader());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
