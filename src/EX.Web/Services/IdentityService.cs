using System;
using System.Threading.Tasks;
using EX.Common;
using EX.Common.Dtos;
using EX.Common.Rest;
using EX.Web.Dtos;

namespace EX.Web.Services
{
    public class IdentityService : ApiClient
    {
        private readonly AppConfig _config;

        public IdentityService(AppConfig config) : base(config.IdentityApi)
        {
            _config = config;
        }

        public async Task<UserEntityDto> GetUser(string id)
        {
            try
            {
                return await Get<UserEntityDto>($"/User/Get/{id}");
            }
            catch
            {
                return new UserEntityDto();
            }
        }

        public Task<UserTokenDto> GetToken(string token)
        {
            return Post<UserTokenDto>($"/Account/Token?client={_config.ClientId}", new
            {
                refreshToken = token
            });
        }


    }
}
