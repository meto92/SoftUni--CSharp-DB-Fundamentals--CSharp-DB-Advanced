using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class CategoryDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}