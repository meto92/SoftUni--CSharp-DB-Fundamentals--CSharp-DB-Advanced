using System.Xml.Serialization;

namespace PlanetHunters.DataExporter.DTOs
{
    public class DiscoveryInfoDto
    {
        [XmlAttribute]
        public string DiscoveryDate { get; set; }

        [XmlAttribute]
        public string TelescopeName { get; set; }
    }
}