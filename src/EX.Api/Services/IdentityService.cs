using System.Threading.Tasks;
using EX.Common;
using EX.Common.Dtos;
using EX.Common.Rest;

namespace EX.Api.Services
{
    public class IdentityService : ApiClient
    {
        public IdentityService(AppConfig config) : base(config.IdentityApi)
        {
        }

        public Task<UserTokenDto> LogIn(string username, string password)
        {
            return Post<UserTokenDto>("/Account/Login", new
            {
                username,
                password
            });
        }

        public Task<UserInfoDto> GetInfo(string token)
        {
            return Get<UserInfoDto>("/Account/Info", token);
        }

        public Task LogOut(string token)
        {
            return Post("/Account/Logout", token);
        }

    }
}
