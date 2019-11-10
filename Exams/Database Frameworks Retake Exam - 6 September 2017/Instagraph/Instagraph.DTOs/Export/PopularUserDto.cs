using Newtonsoft.Json;

namespace Instagraph.DTOs.Export
{
    public class PopularUserDto
    {
        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("followers")]
        public int FollowersCount { get; set; }
    }
}