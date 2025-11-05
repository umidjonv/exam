using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EX.Desktop.Services
{
    public class SessionService : BaseService
    {
        public Task Set(string token, string session)
        {
            string ipAddress;

            try
            {
                var hostName = Dns.GetHostName();
                var ipAddresses = Dns.GetHostEntry(hostName).AddressList;

                ipAddress = $"{ipAddresses.FirstOrDefault()}";
            }
            catch
            {
                ipAddress = "127.0.0.1";
            }

            return Post($"/session/set/{session}", new
            {
                ipAddress,
                computerDomain = Environment.UserDomainName,
                computerName = Environment.MachineName,
                computerUser = Environment.UserName
            }, token);
        }

        public Task Active(string sessionId, string audioId, string videoId)
        {
            return Post($"/session/active/{sessionId}", new
            {
                videoDriver = videoId,
                audioDriver = audioId
            });
        }
    }
}