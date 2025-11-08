using EX.Common;
using EX.Common.Auth;
using EX.Common.Helpers;
using EX.Data;
using EX.Data.Core;
using EX.Web.Consts;
using EX.Web.Extensions;
using EX.Web.Filters;
using EX.Web.Helpers;
using EX.Web.Jobs;
using EX.Web.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using UzEx.Storage.MinIO;
using WebMarkupMin.AspNetCore3;

namespace EX.Web
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {

            if (environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

                // Disabled: NetworkHelper.ConfigureProxy() causes slow startup due to DNS lookups
                // NetworkHelper.ConfigureProxy();
            }

            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureNonBreakingSameSiteCookies();

            var config = new AppConfig();
            Configuration.GetSection("Config").Bind(config);
            services.AddSingleton(config);

            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();
            //services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<HandbookService>();
            services.AddTransient<ExamService>();
            services.AddTransient<SmsService>();
            services.AddTransient<IdentityService>();

            // Disabled: RedisManagerPool can cause slow startup if Redis is unavailable
            // services.AddSingleton(provider => new RedisManagerPool(config.RedisConnection));

            // Disabled: CacheService depends on RedisManagerPool
            // services.AddTransient<CacheService>();

            // Disabled: MinStorageClient can cause slow startup if MinIO is unavailable
            // services.AddTransient(a => new MinStorageClient(config.CloudEndpoint));

            // Disabled: ExamJob requires database which is currently disabled
            // services.AddHostedService<ExamJob>();

            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });
            services.Configure<RequestLocalizationOptions>(a =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("uz")
                };

                a.DefaultRequestCulture = new RequestCulture("uz");
                a.SupportedCultures = supportedCultures;
                a.SupportedUICultures = supportedCultures;
                a.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            services.AddControllersWithViews()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts =>
            {
                opts.ResourcesPath = "Resources";
            })
            .AddDataAnnotationsLocalization();

            // Configure Bearer Authentication with no expiration for API
            // and Cookie Authentication for web pages
            var jwtSecretKey = Configuration["Jwt:SecretKey"];
            var jwtIssuer = Configuration["Jwt:Issuer"];
            var jwtAudience = Configuration["Jwt:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
                options.AccessDeniedPath = "/error";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Disable token expiration validation
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConst.AdminPolicy, builder => builder.RequireClaim(AuthConst.UserClaim, AuthConst.AdminRole).RequireAuthenticatedUser());
                options.AddPolicy(AuthConst.ClientPolicy, builder => builder.RequireAssertion(context => context.User.FindFirstValue(AuthConst.UserClaim) != AuthConst.AdminRole).RequireAuthenticatedUser());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.SetIsOriginAllowed(a => true)
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            if (Environment.IsProduction())
            {
                //services.AddWebMarkupMin(options =>
                //    {
                //        options.AllowMinificationInDevelopmentEnvironment = true;
                //        options.AllowCompressionInDevelopmentEnvironment = true;
                //        options.DisablePoweredByHttpHeaders = true;
                //    })
                //    .AddHtmlMinification(options =>
                //    {
                //        options.MinificationSettings.RemoveRedundantAttributes = true;
                //        options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                //        options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                //    })
                //    .AddHttpCompression();
            }

            //services.AddHealthChecks()
            //    .AddMySql(Configuration.GetConnectionString("DefaultConnection"))
            //    .AddRedis(config.RedisConnection)
            //    .AddCheck<StorageHealthCheck>("storage");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            if (env.IsProduction())
            {
                //app.UseWebMarkupMin();
            }

            if (Environment.IsDevelopment())
            {
                //app.UseSerilogRequestLogging();
            }

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("health", new HealthCheckOptions
                //{
                //    Predicate = _ => true,
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller}/{action}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();
            });
        }

    }
}
