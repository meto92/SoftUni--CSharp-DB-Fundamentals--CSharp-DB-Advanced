using System;

namespace PetClinic.Models
{
    public class Passport
    {
        public string SerialNumber { get; private set; }

        public Animal Animal { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerName { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}