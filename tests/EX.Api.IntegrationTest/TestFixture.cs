using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using EX.Common.Rest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EX.Api.IntegrationTest
{
    public class TestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;

        public TestFixture()
            : this(Environment.CurrentDirectory)
        {
        }

        protected TestFixture(string contentRoot)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .AddEnvironmentVariables();

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(InitializeServices)
                .UseConfiguration(configurationBuilder.Build())
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup));

            _server = new TestServer(webHostBuilder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiResponseType.JsonResponse));
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
         
        protected virtual void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(startupAssembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider(),
                    new ViewComponentFeatureProvider()
                }
            };

            services.AddSingleton(manager);
        }
    }
}
