using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace EX.Common.Helpers
{
    public class NetworkHelper
    {

        private const string LocaleDomain = "rtsb.uz";

        public static void ConfigureProxy()
        {

            ServicePointManager.ServerCertificateValidationCallback += (a, b, c, d) => true;
             
            if (IsLocale())
            {
#if NET48
#else
                HttpClient.DefaultProxy = GetDefaultProxy();
#endif
            }

        }
        
        public static bool IsLocale()
        {

            var lan = IPGlobalProperties.GetIPGlobalProperties();
            var domain = lan.DomainName;

            return domain == LocaleDomain;
        }

        public static HttpClientHandler ConfigureClientHandler()
        {

            ServicePointManager.ServerCertificateValidationCallback += (a, b, c, d) => true;
             
            if (IsLocale())
            {
                return new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = GetDefaultProxy(),
                };
            }

            return new HttpClientHandler();
        }

        public static WebProxy GetDefaultProxy() => new WebProxy("http://192.168.15.3:8080", true)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("e.latipov", "web@1234")
        };

    }
}
