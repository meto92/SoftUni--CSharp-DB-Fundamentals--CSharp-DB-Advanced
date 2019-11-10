using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    public class DiscoveryStarDto
    {
        [XmlText]
        public string StarName { get; set; }
    }
}