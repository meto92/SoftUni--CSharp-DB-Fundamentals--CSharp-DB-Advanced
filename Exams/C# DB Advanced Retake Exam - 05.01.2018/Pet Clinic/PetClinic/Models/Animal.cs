using System.Collections.Generic;

namespace PetClinic.Models
{
    public class Animal
    {
        public Animal()
        {
            this.Procedures = new HashSet<Procedure>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Age { get; set; }

        public string PassportSerialNumber { get; set; }

        public Passport Passport { get; set; }

        public ICollection<Procedure> Procedures { get; set; }
    }
}