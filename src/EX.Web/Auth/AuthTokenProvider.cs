using EX.Common.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EX.Common.Auth
{
    public class AuthTokenProvider:IAuthTokenProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthTokenProvider> _logger;

        private UserTokenDto _currentToken;
        private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public AuthTokenProvider(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AuthTokenProvider> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            // небольшой запас по времени
            if (_currentToken != null && _expiresAt > DateTimeOffset.UtcNow.AddSeconds(30))
                return _currentToken.AccessToken;

            await _lock.WaitAsync(cancellationToken);
            try
            {
                // ещё раз проверим внутри локера
                if (_currentToken != null && _expiresAt > DateTimeOffset.UtcNow.AddSeconds(30))
                    return _currentToken.AccessToken;

                var client = _httpClientFactory.CreateClient("AuthApi");

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _configuration["Auth:ClientId"],
                    ["client_secret"] = _configuration["Auth:ClientSecret"],
                    // если нужен scope:
                    // ["scope"] = "openid profile"
                };

                var response = await client.PostAsync(
                    _configuration["Auth:TokenEndpoint"], // например: "/realms/demo/protocol/openid-connect/token"
                    new FormUrlEncodedContent(form),
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                _currentToken = JsonConvert.DeserializeObject<UserTokenDto>(json)
                                ?? throw new InvalidOperationException("Cannot deserialize token response");

                _expiresAt = DateTimeOffset.UtcNow.AddSeconds(_currentToken.ExpiresIn);

                _logger.LogInformation("Got new access token, expires at {ExpiresAt}", _expiresAt);

                return _currentToken.AccessToken;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
