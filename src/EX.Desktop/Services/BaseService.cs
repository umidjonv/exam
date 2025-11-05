using EX.Common.Rest;
using EX.Desktop.Helpers;

namespace EX.Desktop.Services
{
    public abstract class BaseService : ApiClient
    {
        protected BaseService() : base(AppSettings.ApiUrl, ProxyHelper.DefaultProxy)
        {
        }
    }
}