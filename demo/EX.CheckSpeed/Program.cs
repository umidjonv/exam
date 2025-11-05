using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace EX.CheckSpeed
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var hostName = "uzex.uz";

            Console.Title = $"{hostName} - Speed report";

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Console.WriteLine("No internet connection!");
            }
            else
            {
                // ping
                Console.WriteLine("Ping...");
                var ping = new Ping();
                var startTime = DateTime.Now;
                var reply = ping.Send(hostName,1000);
                var finishTime = DateTime.Now;
                if (reply != null &&
                    reply.Status == IPStatus.Success &&
                    Math.Round((finishTime - startTime).TotalSeconds, 0) <= 1)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    Console.WriteLine("TIMEOUT");
                }
                Console.WriteLine();

                // lan
                Console.WriteLine("Network: ");
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var avgSpeed = networkInterfaces.Average(a => a.Speed);
                Console.WriteLine("{0:F}MB/sec", avgSpeed / 1024 / 1024 / 1024);
                Console.WriteLine();

                // download
                Console.WriteLine("Downloading...");
                var beginTime = DateTime.Now;
                var client = new WebClient(); 
                client.DownloadDataCompleted += (sender, eventArgs) =>
                {
                    var endTime = DateTime.Now;
                    var data = eventArgs.Result;
                    var speed = (double)data.LongLength / 1024 / 1024 / (endTime - beginTime).TotalSeconds;

                    Console.WriteLine("{0:F}MB/sec", speed);
                };
                client.DownloadDataAsync(new Uri($"http://{hostName}/files/videos/%D0%9A%D0%BE%D1%80%D0%B0%20%D0%BC%D0%B5%D1%82%D0%B0%D0%BB%D0%BB%20%D1%81%D0%B5%D0%BD%D1%82%D1%8F%D0%B1%D1%80%D1%8C%202017%20%D0%B9.mp4"));
                Console.WriteLine();

            }

            Console.ReadKey();
        }

    }
}