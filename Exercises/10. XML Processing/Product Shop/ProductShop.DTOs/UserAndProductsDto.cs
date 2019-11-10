using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    public class UserAndProductsDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

        [XmlIgnore]
        public ProductDto[] SoldProducts { get; set; }

        [XmlElement("sold-products")]
        public SoldProductsDto SoldProductsDto { get; set; }
    }
}