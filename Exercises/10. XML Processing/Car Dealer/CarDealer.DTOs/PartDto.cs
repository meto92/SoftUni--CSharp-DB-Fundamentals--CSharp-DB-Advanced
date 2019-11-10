using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("part")]
    public class PartDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlIgnore]
        [XmlAttribute("quantity")]
        public int Quantity { get; set; }
    }
}