using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("product")]
    public class ProductWithBuyerDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("buyer")]
        public string Buyer { get; set; }
    }
}