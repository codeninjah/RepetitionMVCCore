using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repetition.Data;
using Repetition.Models;
using Repetition.Services;
using Repetition.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Repetition
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

			// DETTA ÄR TILLLAGD
			ITimeProvider myFakeTimeProvider = new FakeTimeProvider();
			myFakeTimeProvider.Now = new DateTime(2018, 2, 1);
			services.AddSingleton<ITimeProvider>(myFakeTimeProvider);
			// för realtime adda (new RealTimeProvider) istället för myFakeTimeProvider
			//UNTIL HERE

			//läggs till för localization
			services.AddLocalization(options => options.ResourcesPath = "");
			services.AddMvc()
			//LÄGGS TILL FÖR LOCALIZATION
			.AddViewLocalization()
				.AddDataAnnotationsLocalization();
			//SLUT PÅ LOCALITZATION
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
			,ApplicationDbContext context, UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
        {
			//app.Use((context2, next) =>
			//{
			//	var cultureQuery = context2.Request.Query["culture"];
			//	if (!string.IsNullOrWhiteSpace(cultureQuery))
			//	{
			//		var culture = new CultureInfo(cultureQuery);
			//		CultureInfo.CurrentCulture = culture;
			//		CultureInfo.CurrentUICulture = culture;
			//	}
			//	else
			//	{
			//		var culture = new CultureInfo("en-US");
			//		CultureInfo.CurrentCulture = culture;
			//		CultureInfo.CurrentUICulture = culture;
			//	}

			//	return next();
			//});
				
			//LÄSS TILL FÖR CULTURER
			List<CultureInfo> supportedCultures = new List<CultureInfo>
			{
				new CultureInfo("sv-SE"),
				new CultureInfo("en-US")
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("sv-SE"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});
			//SLUT 

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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{slug?}");
            });

			DbSeeder.Seed(context, userManager, roleManager);
		}
    }
}
