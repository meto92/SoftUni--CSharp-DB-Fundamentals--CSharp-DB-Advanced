using System.Xml.Serialization;

namespace PetClinic.DTOs
{
    [XmlType("Procedure")]
    public class ExportProcedureDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public ExportAnimalAidDto[] AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }
    }
}