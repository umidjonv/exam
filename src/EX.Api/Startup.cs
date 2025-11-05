using System;
using System.Net;
using EX.Api.Extensions;
using EX.Api.Filters;
using EX.Api.Hubs;
using EX.Api.Jobs;
using EX.Api.Services;
using EX.Common;
using EX.Common.Helpers;
using EX.Common.Rest;
using EX.Data;
using EX.Data.Core;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using Swashbuckle.AspNetCore.SwaggerUI;
using UzEx.Storage.MinIO;

namespace EX.Api
{

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

                NetworkHelper.ConfigureProxy();
            }

            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = new AppConfig();
            Configuration.GetSection("Config").Bind(config);
            services.AddSingleton(config);

            services.AddHttpContextAccessor();
            services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
           
            services.AddHostedService<TimerJob>();
            services.AddTransient<ExamService>();
            services.AddTransient<IdentityService>();
            services.AddTransient(a => new MinStorageClient(config.CloudEndpoint));

            services.AddControllers();
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //      {
            //          options.MetadataAddress = $"{config.TokenUrl}/auth/realms/{config.Realm}/.well-known/openid-configuration";
            //          options.RequireHttpsMetadata = false;
            //          options.IncludeErrorDetails = true;

            //          options.TokenValidationParameters = new TokenValidationParameters
            //          {
            //              ClockSkew = TimeSpan.FromMinutes(5),
            //              ValidateLifetime = true,
            //              ValidateAudience = false,
            //              ValidateIssuerSigningKey = true,
            //              ValidateTokenReplay = true,
            //              ValidateActor = false,
            //              ValidateIssuer = true,
            //              ValidIssuer = $"{config.TokenUrl}/auth/realms/{config.Realm}"
            //          };
            //          options.Events = new JwtBearerEvents()
            //          {
            //              OnAuthenticationFailed = context =>
            //              {
            //                  context.NoResult();

            //                  var message = context.Exception.InnerException?.Message ?? context.Exception.Message;
            //                  var error = new ApiResponse
            //                  {
            //                      Error = message,
            //                      Success = false
            //                  };

            //                  context.Response.StatusCode = (int)HttpStatusCode.OK;
            //                  context.Response.ContentType = ApiResponseType.JsonResponse;

            //                  return context.Response.WriteAsync(error.ToString());
            //              } 
            //          };
            //      });
            //services.AddAuthorization();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.SetIsOriginAllowed(a => true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{config.Title} API",
                    Version = $"v1"
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

            });

            services.AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString("DefaultConnection"))
                .AddCheck<StorageHealthCheck>("storage");
        }

        public void Configure(IApplicationBuilder app, AppConfig config)
        {

            app.UseAppException();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHub<StreamHub>("/hub/stream");
                endpoints.MapHub<TimerHub>("/hub/timer");
                endpoints.MapHub<ExamHub>("/hub/exam");

                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.ShowExtensions();
                c.EnableValidator();

                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{config.Title} API V1");
            });
        }
    }
}
