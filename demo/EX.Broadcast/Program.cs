using EX.Common.Helpers;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace EX.Broadcast
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://sp-exam-api.uzex.uz/hub/timer",a=>
                {
                    a.Proxy = NetworkHelper.GetDefaultProxy();
                })
                .Build();
            connection.On<DateTime>("time-now", (time) =>
            {
                Console.Write($"{time}\r");
            });

            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

            await connection.StopAsync();
        }
    }
}
