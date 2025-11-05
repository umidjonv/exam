using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EX.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Exam UI";

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hst, cnf) =>
                    {
                        cnf.ReadFrom.Configuration(hst.Configuration);
                        cnf.Enrich.FromLogContext();
                        cnf.Enrich.WithProperty("ApplicationName", hst.HostingEnvironment.ApplicationName);
                        cnf.WriteTo.Console();
                        cnf.WriteTo.Debug();
                        cnf.WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
