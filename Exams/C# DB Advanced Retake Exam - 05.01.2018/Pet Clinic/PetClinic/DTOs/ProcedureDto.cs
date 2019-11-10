using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DTOs
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [Required]
        [XmlElement("Vet")]
        public string VetName { get; set; }

        [Required]
        [XmlElement("Animal")]
        public string AnimalSerialNumber { get; set; }

        [XmlIgnore]
        public DateTime DateTime { get; set; }

        [Required]
        [XmlElement(nameof(DateTime))]
        public string Date
        {
            get { return this.DateTime.ToString("dd-MM-yyyy"); }
            set { this.DateTime = DateTime.Parse(value); }
        }

        [XmlArray("AnimalAids")]
        public AnimalAidDto[] AnimalAids { get; set; }
    }
}