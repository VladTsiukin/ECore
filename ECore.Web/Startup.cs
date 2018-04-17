using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ECore.Domain.Context;
using ECore.Domain.Entities;
using ECore.Service.Utils;
using ECore.Web.Infrastructure;
using ECore.Web.BLService;


namespace ECore.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            configuration = Configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            //if (string.IsNullOrEmpty(Configuration["Authentication:Google:client_id"]))
            //{
            //    throw new InvalidOperationException("User secrets must be configured for each authentication provider.");
            //}

            // add services
            services.AddTransient<IPasswordValidator<AppUser>, PasswordValidator>();
            services.AddTransient<IUserValidator<AppUser>, AppUserValidator>();

            // add trenning service
            services.AddScoped<ITrainingService, Training>();

            // add cache:
            services.AddMemoryCache();

            // add mvc:
            services.AddMvc();

            // add db:
            services.AddDbContext<EcoreDbContext>(optoins =>
                optoins.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly("ECore.Domain")));

            // add identity:
            services.AddIdentity<AppUser, IdentityRole>(opts =>
                {
                    // user
                    opts.User.RequireUniqueEmail = false;
                    opts.User.AllowedUserNameCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                    // password
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                    //sign in
                    opts.SignIn.RequireConfirmedEmail = true;
                    // Lockout settings
                    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    opts.Lockout.MaxFailedAccessAttempts = 10;
                    opts.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<EcoreDbContext>()
                .AddDefaultTokenProviders();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // google:
            services.AddAuthentication().AddGoogle(op =>
            {
                op.ClientId = Configuration["Authentication:Google:client_id"];
                op.ClientSecret = Configuration["Authentication:Google:client_secret"];
            });

            // facebook
            services.AddAuthentication().AddFacebook(op =>
            {
                op.ClientId = Configuration["Authentication:Facebook:client_id"];
                op.ClientSecret = Configuration["Authentication:Facebook:client_secret"];
            });

            // authorization
            services.AddAuthorization(op =>
            {
                op.AddPolicy("OnlyForAdmin", policy =>
                {
                    policy.RequireRole("adminvictestvt");
                });
            });

            //Add AutoFac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<ECoreAutofacModul>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              ILoggerFactory loggerFactory)
        {
            // add logger:
            loggerFactory.AddConsole(Configuration.GetSection("Loging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseAntiforgeryTokens();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            // Initialize data:
            //ECore.Domain.Context.ECoreDbInitializer.Initialize(dbContext);
        }
    }
}
