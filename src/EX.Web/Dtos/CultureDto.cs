using Newtonsoft.Json;

namespace EX.Web.Dtos
{
    public class CultureDto
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

    }
}