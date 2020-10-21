using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Business.Services;
using GaripSozluk.Data;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using GaripSozluk.Data.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GaripSozluk.Api
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

            var hangfireConnectionString = Configuration.GetConnectionString("AppDatabaseHangfire");
            services.AddHangfire(config =>
            {
                var option = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromMinutes(5),
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                };
                config.UseSqlServerStorage(hangfireConnectionString, option)
                .WithJobExpirationTimeout(TimeSpan.FromHours(6));

            });
            services.AddHangfireServer();

            //services.AddHangfire(configuration => configuration
            //        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //        .UseSimpleAssemblyNameTypeSerializer()
            //        .UseRecommendedSerializerSettings()
            //        .UseSqlServerStorage(Configuration.GetConnectionString("AppDatabaseHangfire"), new SqlServerStorageOptions
            //        {
            //            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //            QueuePollInterval = TimeSpan.Zero,
            //            UseRecommendedIsolationLevel = true,
            //            DisableGlobalLocks = true
            //        }));

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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "GaripSozluk Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint($"/swagger/v1/swagger.json", "GaripSozluk Service V1");
             });

            app.UseHangfireDashboard();
            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!")*/);
            RecurringJob.AddOrUpdate<IPostService>(x => x.AddLogPost(), "0 1 * * *", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IPostService>(x => x.AddLogPostFilter(), "10 1 * * *", TimeZoneInfo.Local);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
