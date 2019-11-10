using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    public class PioneerDto
    {
        [XmlText]
        public string Pioneer { get; set; }

        public string FirstName => Pioneer.Split(", ")[1];

        public string LastName => Pioneer.Split(", ")[0];
    }
}