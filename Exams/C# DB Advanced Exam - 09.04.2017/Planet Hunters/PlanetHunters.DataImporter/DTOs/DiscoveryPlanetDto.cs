using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    public class DiscoveryPlanetDto
    {
        [XmlText]
        public string PlanetName { get; set; }
    }
}