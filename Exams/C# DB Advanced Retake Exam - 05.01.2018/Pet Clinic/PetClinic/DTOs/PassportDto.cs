using System;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.DTOs
{
    public class PassportDto
    {
        [RegularExpression(@"^\p{L}{7}\d{3}$")]
        public string SerialNumber { get; set; }

        [RegularExpression(@"^(\+359|0)\d{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string OwnerName { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}