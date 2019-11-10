using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class SoldProductsDto
    {
        [JsonProperty("count")]
        public int ProductsCount { get; set; }

        [JsonProperty("products")]
        public ProductDto[] Products { get; set; }
    }
}