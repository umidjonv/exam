using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EX.Common.Dtos;
using EX.Common.Rest;

namespace EX.Desktop.Services
{
    public class ExamService : BaseService
    {

        public Task<ExamStateDto> CheckStatus(string userId)
        {
            return Post<ExamStateDto>($"/Exam/CheckStatus/{userId}", null);
        }

        public async Task<bool> Upload(string userId, string sessionId, byte[] data)
        {
            var client = CreateHttpClient();
            var form = new MultipartFormDataContent
            {
                {new ByteArrayContent(data), "file", "file1"}
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

            var request = await client.PostAsync($"/Exam/Upload/{userId}?session={sessionId}", form);

            if (!request.IsSuccessStatusCode)
                throw new Exception(request.ReasonPhrase);

            var response = await request.Content.ReadFromJsonAsync<ApiResponse>();

            return response.Success;
        }
    }
}