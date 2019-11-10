using System.Xml.Serialization;

namespace PetClinic.DTOs
{
    [XmlType("AnimalAid")]
    public class ExportAnimalAidDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}