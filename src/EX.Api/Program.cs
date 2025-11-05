using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace EX.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {

            Console.Title = "Exam Api";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName)
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File("Logs/api.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();

            try
            {
                Console.Title = "Exam Api";
                Log.Information("Starting web host (Environment: {Environment})", configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production");

                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                // Catch startup exceptions and ensure they are logged.
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                // Ensure buffered events are flushed.
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseSerilog((hst, cnf) =>
                    //{
                    //    cnf.ReadFrom.Configuration(hst.Configuration);
                    //    cnf.Enrich.FromLogContext();
                    //    cnf.Enrich.WithProperty("ApplicationName", hst.HostingEnvironment.ApplicationName);
                    //    cnf.WriteTo.Console();
                    //    cnf.WriteTo.Debug();
                    //    cnf.WriteTo.File("Logs/api.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
                    //});

                    webBuilder.UseStartup<Startup>();
                });
    }
}
