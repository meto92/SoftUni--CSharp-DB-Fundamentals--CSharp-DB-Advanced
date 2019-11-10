namespace ProductShop.DTOs
{
    public class ProductWithCategoryIdsDto
    {
        public decimal Price { get; set; }

        public int[] CategoryIds { get; set; }
    }
}