using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class UsersAndProductsDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public UserAndProductsDto[] Users { get; set; }
    }
}