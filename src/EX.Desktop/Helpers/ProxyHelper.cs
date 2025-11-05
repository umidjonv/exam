using System.Net;
using EX.Desktop.Properties;

namespace EX.Desktop.Helpers
{
    internal static class ProxyHelper
    {
        public static IWebProxy DefaultProxy => Settings.Default.IsDefault
            ? WebProxy.GetDefaultProxy()
            : new WebProxy(Settings.Default.Host, Settings.Default.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Settings.Default.User, Settings.Default.Password)
            };
         
        public static bool Call()
        {
            try
            {
                using var wc = new WebClient
                {
                    Proxy = DefaultProxy
                };
                var data = wc.DownloadString($"{AppSettings.ApiUrl}/swagger");

                return !string.IsNullOrWhiteSpace(data);
            }
            catch
            {
                return false;
            }
        }
    }
}