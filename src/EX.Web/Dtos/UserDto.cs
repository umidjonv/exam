using System.Collections.Generic;
using Newtonsoft.Json;

namespace EX.Web.Dtos
{
    public class UserEntityDto
    {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("createdTimestamp")]
        public long CreatedTimestamp { get; set; }
        
        [JsonProperty("username")]
        public string UserName { get; set; }
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty("totp")]
        public bool? Totp { get; set; }
        
        [JsonProperty("emailVerified")]
        public bool? EmailVerified { get; set; }
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("attributes")]
        public Dictionary<string, IEnumerable<string>> Attributes { get; set; }

    }
}
