using System.Threading.Tasks;
using EX.Common.Dtos;

namespace EX.Desktop.Services
{
    public class MembershipService : BaseService
    {
        public Task<UserTokenDto> GetToken(string username, string password)
        {
            return Post<UserTokenDto>("/account/token", new
            {
                username,
                password
            });
        }

        public Task<UserInfoDto> GetInfo(string token)
        {
            return Get<UserInfoDto>("/account/info", token);
        }

        public Task LogOff(string token)
        {
            return Post("/account/logoff", token);
        }
    }
}