using Newtonsoft.Json;

namespace EX.Web.Dtos
{
    public class RegionDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }
    }
}
