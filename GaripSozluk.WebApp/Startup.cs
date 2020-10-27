using System;
using System.Linq;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Business.Middleware;
using GaripSozluk.Business.Services;
using GaripSozluk.Common.healthcheck;
using GaripSozluk.Data;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.HealthCheck;
using GaripSozluk.Data.Interfaces;
using GaripSozluk.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GaripSozluk.WebApp
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
            var connectionString = Configuration.GetConnectionString("AppDatabase");
            services.AddDbContext<GaripSozlukDbContext>(options => options.UseSqlServer(connectionString));

            var connectionStringPostgre = Configuration.GetConnectionString("AppDatabasePostgre");
            services.AddDbContext<GaripSozlukDbContextLog>(options => options.UseNpgsql(connectionStringPostgre));

            services.AddHealthChecks()
           //.AddDbContextCheck<GaripSozlukDbContext>()
           .AddCheck<SqlServerHealthCheck>(name: "New Custom Check");
            services.AddHealthChecksUI(setupSettings: settings =>
            {
                settings.AddHealthCheckEndpoint("health", "http://localhost:54598/health");
            }).AddInMemoryStorage();
            //services.AddHealthChecks().AddDbContextCheck<GaripSozlukDbContext>().AddCheck<SqlServerHealthCheck>(name:"sql status"); 
            //AddCheck().AddDbContextCheck<GaripSozlukDbContext>();


            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IPostCategoryService, PostCategoryService>();
            services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IBlockedUserService, BlockedUserService>();
            services.AddScoped<IBlockedUserRepository, BlockedUserRepository>();

            services.AddScoped<IRatingRepository, RatingRepository>();

            services.AddScoped<IApiService, ApiService>();

            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILogRepository, LogRepository>();

            services.AddHttpContextAccessor();


            services.AddIdentity<User, Role>()
         .AddRoles<Role>()
         .AddRoleManager<RoleManager<Role>>()
         .AddEntityFrameworkStores<GaripSozlukDbContext>();
            services.ConfigureApplicationCookie(options =>
            {

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "";
                options.SlidingExpiration = true;
            });
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                        new CultureInfo("en-US"),
                       new CultureInfo("tr-TR")
                };

                //options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
               {
                   new QueryStringRequestCultureProvider(),
                   //new CookieRequestCultureProvider(),
                   //new AcceptLanguageHeaderRequestCultureProvider()
                };
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var userLangs = context.Request.GetTypedHeaders().AcceptLanguage;
                    var cultureItem = context.Request.RouteValues.Where(x => x.Key == "culture").FirstOrDefault();
                    var culture = cultureItem.Value == null ? "tr-TR" : cultureItem.Value.ToString(); // kültür null ise tr olacak değilse gelen değeri seçecek
                    return Task.FromResult(new ProviderCultureResult(culture, culture));
                }));

            });
            services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => { options.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<LogMiddleware>();

            app.UseHealthChecksUI();
            var localizedOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizedOptions.Value);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "culture",
                  pattern: "{culture?}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,

                });
                endpoints.MapHealthChecksUI(options => { options.UIPath = "/healthchecks-ui"; options.ApiPath = "/healthchecks-ui-api"; });

            });
        }
    }
}
