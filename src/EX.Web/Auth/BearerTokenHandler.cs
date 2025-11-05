using EX.Common.Auth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EX.Web.Auth
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IAuthTokenProvider _tokenProvider;

        public BearerTokenHandler(IAuthTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
