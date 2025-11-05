using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EX.Common.Auth
{
    public interface IAuthTokenProvider
    {
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
