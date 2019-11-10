using System.Collections.Generic;

namespace ProductShop.Models
{
    public class Product
    {
        public Product()
        {
            this.ProductCategories = new HashSet<CategoryProduct>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

        public virtual User Seller { get; set; }

        public virtual User Buyer { get; set; }

        public virtual ICollection<CategoryProduct> ProductCategories { get; set; }
    }
}