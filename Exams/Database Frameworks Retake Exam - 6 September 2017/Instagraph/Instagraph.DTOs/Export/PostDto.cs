using Newtonsoft.Json;

namespace Instagraph.DTOs.Export
{
    public class PostDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }
}