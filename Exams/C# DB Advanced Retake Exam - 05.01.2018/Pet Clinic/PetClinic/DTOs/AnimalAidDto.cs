using System.Xml.Serialization;

namespace PetClinic.DTOs
{
    [XmlType("AnimalAid")]
    public class AnimalAidDto
    {
        public string Name { get; set; }
    }
}