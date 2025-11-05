using System.Diagnostics;

namespace EX.Common.Helpers
{
    public static class ProcessHelper
    {
        public static void OpenBrowser(string url)
        {
            try
            {
                OpenFile(url);
            }
            catch
            {
                ExecuteCommand(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    CreateNoWindow = true
                });
            }
        }

        public static void OpenFile(string url)
        {
            Process.Start(url);
        }

        public static void ExecuteCommand(ProcessStartInfo info, bool wait = false)
        {
            if (wait)
            {
                using var exeProcess = Process.Start(info);

                exeProcess.WaitForExit();
            }
            else
            {
                Process.Start(info);
            }
        }

    }
}
