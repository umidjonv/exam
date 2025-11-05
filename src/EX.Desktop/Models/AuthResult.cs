using EX.Common.Dtos;

namespace EX.Desktop.Models
{
    public class AuthResult
    {
        public UserInfoDto Info { get; set; }

        public UserTokenDto Token { get; set; }
    }
}