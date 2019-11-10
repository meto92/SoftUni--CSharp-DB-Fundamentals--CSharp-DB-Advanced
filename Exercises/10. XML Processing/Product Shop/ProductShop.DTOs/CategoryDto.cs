using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("category")]
    public class CategoryDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }
}