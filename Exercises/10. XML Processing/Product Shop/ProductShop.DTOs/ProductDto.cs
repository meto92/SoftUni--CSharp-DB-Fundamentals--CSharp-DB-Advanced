using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("product")]
    public class ProductDto
    {
        [XmlAttribute("name")]
        //[XmlElement("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        //[XmlElement("price")]
        public decimal Price { get; set; }

        [XmlIgnore]
        public int[] CategoryIds { get; set; }
    }
}