using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using NAudio.CoreAudioApi;

namespace EX.Desktop.Helpers
{
    public static class RecorderHelper
    {

        private static bool _kill;

        public static IDictionary<string, string> GetVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            return devices.Cast<FilterInfo>().ToDictionary(a => a.Name, a => a.MonikerString);
        }

        public static IDictionary<string, string> GetAudioDevices()
        {
            var reader = new MMDeviceEnumerator();
            var devices = reader.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            return devices.ToDictionary(a => a.FriendlyName, a => a.DeviceFriendlyName);
        }

        public static void Killing()
        {
            _kill = true;
        }

        public static bool Killed()
        {
            return _kill;
        }

        public static string Stream(string audio, TimeSpan wait)
        {
            try
            {
                var path = Path.Combine(AppSettings.AppRoot, "Tmp", "video", Path.ChangeExtension(Path.GetRandomFileName(), ".mp4"));
                var engine = Path.Combine(AppSettings.AppRoot, "Softs", "ffmpeg.exe");
                var elapsed = (int)wait.TotalSeconds;
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = engine,
                        Arguments =$"-f gdigrab -i desktop -f dshow -i audio=\"{audio}\" -draw_mouse 1 -t {elapsed} \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true
                    }
                };

                process.Start();

                if (process.HasExited)
                    throw new Exception("Кодек не запустился, обратитесь к администратору.");

                ThreadPool.QueueUserWorkItem((a) =>
                {
                    while (!_kill)
                    {
                        Thread.Sleep(AppSettings.DelayTime);
                    }

                    process.StandardInput.WriteLine("q");
                });

                process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();

                return path;
            }
            catch (Exception)
            {
                _kill = true;

                return "";
            }
        }
    }
}