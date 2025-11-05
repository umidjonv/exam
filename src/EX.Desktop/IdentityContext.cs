using System.Security.Claims;
using System.Threading;
using EX.Common.Dtos;

namespace EX.Desktop
{
    public static class IdentityContext
    {
        public static UserInfoDto CurrentUser
        {
            get
            {
                try
                {
                    var identity = (ClaimsIdentity) Thread.CurrentPrincipal.Identity;

                    return new UserInfoDto
                    {
                        Id = identity.FindFirst(ClaimTypes.Sid).Value,
                        Name = identity.FindFirst(ClaimTypes.Name).Value
                    };
                }
                catch
                {
                    return new UserInfoDto();
                }
            }
            set
            {
                var identity = new ClaimsIdentity();

                identity.AddClaim(new Claim(ClaimTypes.Sid, value.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, value.Name));

                Thread.CurrentPrincipal = new ClaimsPrincipal(new[]
                {
                    identity
                });
            }
        }

        public static UserTokenDto UserToken { get; set; }
    }
}