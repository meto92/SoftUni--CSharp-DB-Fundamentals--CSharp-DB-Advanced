using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class UserSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonIgnore]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public SoldProductWithBuyerDto[] ProductsSold { get; set; }
    }
}