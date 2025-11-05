using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EX.Common.Helpers;

namespace EX.Common.Rest
{

    public abstract class ApiClient
    {
        private readonly IWebProxy _proxy;

        protected HttpClient CreateHttpClient(string authToken = "")
        {
            var handler = (_proxy != null)
                ? new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = _proxy
                }
                : NetworkHelper.ConfigureClientHandler();

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiResponseType.JsonResponse));

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            return client;
        }

        private static async Task<T> ConvertResponse<T>(HttpResponseMessage request) where T : class
        {
            if (!request.IsSuccessStatusCode)
                throw new Exception(request.ReasonPhrase);

            var response = await request.Content.ReadFromJsonAsync<ApiResponse<T>>();

            if (!response.Success)
                throw new Exception(response.Error);

            return response.Data;
        }

        private static async Task<object> ConvertResponse(HttpResponseMessage request)
        {
            if (!request.IsSuccessStatusCode)
                throw new Exception(request.ReasonPhrase);

            var response = await request.Content.ReadFromJsonAsync<ApiResponse>();
             
            if (!response.Success)
                throw new Exception(response.Error);

            return response.Data;
        }

        protected ApiClient(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        protected ApiClient(string baseUrl, IWebProxy proxy) : this(baseUrl)
        {
            _proxy = proxy;
        }

        protected string BaseUrl { get; }

        protected virtual async Task<T> Post<T>(string routeUrl, object bodyData, string authToken = "") where T : class
        {
            using var client = CreateHttpClient(authToken);
            var content = new StringContent(JsonConvert.SerializeObject(bodyData), Encoding.UTF8, ApiResponseType.JsonResponse);
            var request = await client.PostAsync(routeUrl, content);

            return await ConvertResponse<T>(request);
        }

        protected virtual async Task<object> Post(string routeUrl, object bodyData, string authToken = "")
        {
            using var client = CreateHttpClient(authToken);
            var content = new StringContent(JsonConvert.SerializeObject(bodyData), Encoding.UTF8, ApiResponseType.JsonResponse);
            var request = await client.PostAsync(routeUrl, content);

            return await ConvertResponse(request);
        }

        protected virtual async Task<T> Get<T>(string routeUrl, string authToken = "") where T : class
        {
            using var client = CreateHttpClient(authToken);
            var request = await client.GetAsync(routeUrl);

            return await ConvertResponse<T>(request);
        }

        protected virtual async Task<object> Get(string routeUrl, string authToken = "")
        {
            using var client = CreateHttpClient(authToken);
            var request = await client.GetAsync(routeUrl);

            return await ConvertResponse(request);
        }

    }

}
