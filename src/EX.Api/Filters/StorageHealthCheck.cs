using System;
using System.Threading;
using System.Threading.Tasks;
using EX.Common;
using EX.Common.Helpers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UzEx.Storage.MinIO;

namespace EX.Api.Filters
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly AppConfig _config;

        public StorageHealthCheck(AppConfig config)
        {
            _config = config;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = new MinStorageClient(_config.CloudEndpoint, NetworkHelper.GetDefaultProxy());

                await client.Search("", "");

                return HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Degraded(exception.Message);
            }
        }

    }
}