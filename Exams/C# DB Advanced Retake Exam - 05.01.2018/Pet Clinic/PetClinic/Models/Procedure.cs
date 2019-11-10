using System;
using System.Collections.Generic;
using System.Linq;

namespace PetClinic.Models
{
    public class Procedure
    {
        public Procedure()
        {
            this.ProcedureAnimalAids = new HashSet<ProcedureAnimalAid>();
        }

        public int Id { get; private set; }

        public int AnimalId { get; set; }

        public int VetId { get; set; }

        public decimal Cost => this.ProcedureAnimalAids.Sum(paa => paa.AnimalAid.Price);

        public DateTime DateTime { get; set; }

        public Animal Animal { get; set; }

        public Vet Vet { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }
    }
}