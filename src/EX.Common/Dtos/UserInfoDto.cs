using Newtonsoft.Json;

namespace EX.Common.Dtos
{ 
    public class UserInfoDto
    {

        [JsonProperty("sub")]
        public string Id { get; set; }

        [JsonProperty("preferred_username")]
        public string Username { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool Verified { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
        
    }
}
