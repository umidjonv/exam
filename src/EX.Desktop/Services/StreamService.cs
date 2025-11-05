using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using EX.Common.Helpers;
using EX.Desktop.Helpers;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace EX.Desktop.Services
{
    public class StreamService : BaseService
    {
        private readonly ExamService _service;
        private bool _aborted;
        private HubConnection _streamConnection;
        private HubConnection _examConnection;
        private ActionBlock<string> _block;

        public async Task Stop()
        {
            _block.Complete();

            await _block.Completion;

            try
            {
                await _streamConnection.StopAsync();
                await _examConnection.StopAsync();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public StreamService(ExamService service)
        {
            _service = service;
        }

        public bool IsConnected => _streamConnection.State == HubConnectionState.Connected;

        public async Task Listen(string userId, string sessionId, string audioDevice, DateTime endTime, Action onFinish)
        {

            // queueing
            _block = new ActionBlock<string>(async file =>
            {

                if (!File.Exists(file))
                    return;

                var dummy = File.ReadAllBytes(file);

                await _service.Upload(userId, sessionId, dummy);

                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    // ignored
                }
            });

            // finishing
            _examConnection = new HubConnectionBuilder()
                .WithUrl($"{AppSettings.ApiUrl}/hub/exam", options =>
                {
                    options.Proxy = ProxyHelper.DefaultProxy;
                })
                .Build();
            _examConnection.On<bool>($"Finish-{userId}", (flag) =>
            {
                if (flag)
                {
                    RecorderHelper.Killing();
                }
            });
            await _examConnection.StartAsync();

            // streaming
            _streamConnection = new HubConnectionBuilder()
                .WithUrl($"{BaseUrl}/hub/stream", options =>
                {
                    options.Headers.Add("user", userId);
                    options.Headers.Add("session", sessionId);
                    options.Proxy = ProxyHelper.DefaultProxy;
                })
                .Build();
            _streamConnection.On($"Listen-{userId}", () =>
            {
                while (!_aborted && IsConnected && !RecorderHelper.Killed())
                {
                    var startTime = DateTime.UtcNow;

                    if (endTime.Subtract(startTime).TotalSeconds < 0)
                        break;

                    var path = RecorderHelper.Stream(audioDevice, AppSettings.SyncTime);

                    if (string.IsNullOrWhiteSpace(path))
                    {
                        break;
                    }

                    _block.Post(path);

                    Thread.Sleep(AppSettings.DelayTime);
                }

                _aborted = true;

                onFinish();

            });
            await _streamConnection.StartAsync();

            await Task.Delay(AppSettings.DelayTime);

            await Task.Run(() =>
            {
                var link = $"{AppSettings.AppUrl}/client/exam/start?session={sessionId}";

                ProcessHelper.OpenBrowser(link);

                Log.Information($"Exam starting by {link}");
            });

            while (IsConnected)
            {
                await Task.Delay(AppSettings.DelayTime);
            }

        }

    }
}