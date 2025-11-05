using EX.Common;
using System.Threading.Tasks;
using EX.Common.Rest;

namespace EX.Web.Services
{
    public class SmsService : ApiClient
    {
        public SmsService(AppConfig config) : base(config.NotificationApi)
        {
        }

        public Task Send(string phone, string message)
        {
            return Post("/Sms/Send", new
            {
                phone,
                message
            });
        }
    }
}
