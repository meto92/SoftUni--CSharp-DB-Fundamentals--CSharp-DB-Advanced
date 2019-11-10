using Newtonsoft.Json;

namespace Instagraph.DTOs.Import
{
    public class UserFollowerDto
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("follower")]
        public string Follower { get; set; }
    }
}