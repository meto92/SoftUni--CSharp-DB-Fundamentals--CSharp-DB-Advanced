using System.Xml.Serialization;

namespace PlanetHunters.DataExporter.DTOs
{
    [XmlType("Star")]
    public class StarDto
    {
        public string Name { get; set; }

        public int Temperature { get; set; }

        public string StarSystem { get; set; }

        public DiscoveryInfoDto DiscoveryInfo { get; set; }
    }
}