using System.Collections.Generic;

namespace PetClinic.Models
{
    public class Vet
    {
        public Vet()
        {
            this.Procedures = new HashSet<Procedure>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Profession { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures { get; set; }
    }
}