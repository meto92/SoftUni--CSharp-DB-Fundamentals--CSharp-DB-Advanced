using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    public class ObserverDto
    {
        [XmlText]
        public string Observer { get; set; }

        public string FirstName => Observer.Split(", ")[1];

        public string LastName => Observer.Split(", ")[0];
    }
}