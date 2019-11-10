using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("car")]
    public class FerrariDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}