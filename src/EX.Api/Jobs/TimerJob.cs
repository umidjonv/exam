using System;
using System.Threading;
using System.Threading.Tasks;
using EX.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace EX.Api.Jobs
{
    public class TimerJob : BackgroundService
    {
        private readonly IHubContext<TimerHub> _hub;

        public TimerJob(IHubContext<TimerHub> hub)
        {
            _hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _hub.Clients.All.SendAsync("time-now", DateTime.UtcNow, cancellationToken: stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}