using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EX.Desktop.Services
{
    public class SpeedService : BaseService
    {
        public async Task<long> Download()
        {
            var client = CreateHttpClient();
            var request = await client.GetAsync(new Uri($"{BaseUrl}/speed/download"));

            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);

            var data = await request.Content.ReadAsStreamAsync();

            return data.Length;
        }

        public async Task Upload(byte[] data)
        {
            var client = CreateHttpClient();
            var form = new MultipartFormDataContent
            {
                {new ByteArrayContent(data), "file", "file1"}
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

            var request = await client.PostAsync($"{BaseUrl}/speed/upload", form);

            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        }
    }
}