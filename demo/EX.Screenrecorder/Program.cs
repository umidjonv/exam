using System;
using System.Diagnostics;
using System.IO;

namespace EX.ScreenRecorder
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {

            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, $"{Guid.NewGuid()}.mp4");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(Environment.CurrentDirectory, "ffmpeg.exe"),
                        Arguments = $"-f gdigrab -i desktop -f dshow -i audio=\"Микрофон (Realtek(R) Audio)\" -draw_mouse 1 -t 60 \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true
                    }
                };

                if (process.HasExited)
                    throw new Exception("Codec didn't run");

                process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();
                Console.WriteLine("Done!");

                Process.Start(path);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
