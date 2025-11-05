using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EX.Common.Dtos;
using EX.Common.Rest;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace EX.Api.IntegrationTest
{
    public class AccountTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public AccountTests(TestFixture<Startup> fixture, ITestOutputHelper output)
        {
            _output = output;
            _client = fixture.Client;
        }

        [Fact]
        public async Task GetToken()
        {
            // Arrange
            var url = "/account/token";
            var body = new
            {
                UserName = "elyor",
                Password = "elyor"
            };

            // Act 
            var request = await _client.PostAsJsonAsync(url, body);

            // Assert
            request.EnsureSuccessStatusCode();

            var response = await request.Content.ReadFromJsonAsync<ApiResponse<UserTokenDto>>();

            response.Should().NotBeNull();
            response.Success.Should().Be(true);
            response.Error.Should().BeNullOrWhiteSpace();

            _output.WriteLine(JsonConvert.SerializeObject(response.Data));

        } 
    }
}
