﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Weapon { get; set; }

        public int DepartmentId { get; set; }

        public OfficerPrisonerDto[] Prisoners { get; set; }
    }
}