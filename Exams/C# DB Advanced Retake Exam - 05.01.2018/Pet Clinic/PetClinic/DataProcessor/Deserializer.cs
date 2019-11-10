namespace PetClinic.DataProcessor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DTOs;
    using PetClinic.Models;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAid[] animalAids = JsonConvert.DeserializeObject<AnimalAid[]>(jsonString);

            List<AnimalAid> animalAidsToAdd = new List<AnimalAid>();

            StringBuilder result = new StringBuilder();

            foreach (AnimalAid animalAid in animalAids)
            {
                if (!IsValid(animalAid) ||
                    animalAidsToAdd.Any(aa => aa.Name == animalAid.Name))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                animalAidsToAdd.Add(animalAid);

                result.AppendLine($"Record {animalAid.Name} successfully imported.");
            }

            context.AnimalAids.AddRange(animalAidsToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalDto[] animals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString, new JsonSerializerSettings { DateFormatString = "dd-MM-yyyy" });

            List<AnimalDto> animalsToAdd = new List<AnimalDto>();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < animals.Length; i++)
            {
                AnimalDto animal = animals[i];

                if (!IsValid(animal) ||
                    !IsValid(animal.Passport) ||
                    animalsToAdd.Any(a => a.Passport.SerialNumber == animal.Passport.SerialNumber))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                animalsToAdd.Add(animal);

                result.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }

            context.Animals.AddRange(animalsToAdd.AsQueryable().ProjectTo<Animal>());

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            XmlRootAttribute root = new XmlRootAttribute("Vets");
            XmlSerializer serializer = new XmlSerializer(typeof(VetDto[]), root);

            VetDto[] vets = null;

            using (StringReader reader = new StringReader(xmlString))
            {
                vets = (VetDto[]) serializer.Deserialize(reader);
            }

            List<VetDto> vetsToAdd = new List<VetDto>();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < vets.Length; i++)
            {
                VetDto vet = vets[i];

                if (!IsValid(vet) ||
                    vetsToAdd.Any(v => v.PhoneNumber == vet.PhoneNumber))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                vetsToAdd.Add(vet);

                result.AppendLine($"Record {vet.Name} successfully imported.");
            }

            context.Vets.AddRange(vetsToAdd.AsQueryable().ProjectTo<Vet>());

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            XmlRootAttribute root = new XmlRootAttribute("Procedures");
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureDto[]), root);

            ProcedureDto[] procedures = null;

            using (StringReader reader = new StringReader(xmlString))
            {
                procedures = (ProcedureDto[]) serializer.Deserialize(reader);
            }

            List<Procedure> proceduresToAdd = new List<Procedure>();

            StringBuilder result = new StringBuilder();

            Dictionary<string, AnimalAid> animalAidByName = procedures
                .Where(p => p.AnimalAids != null && p.AnimalAids.Any())
                .SelectMany(p => p.AnimalAids.Select(aa => aa.Name))
                .Where(a => a != null)
                .Distinct()
                .ToDictionary(aa => aa, aa => context.AnimalAids
                    .FirstOrDefault(animalAid => animalAid.Name == aa));

            foreach (ProcedureDto procedure in procedures)
            {
                Animal animal = context.Animals
                    .Where(a => a.PassportSerialNumber == procedure.AnimalSerialNumber)
                    .FirstOrDefault();

                Vet vet = context.Vets.FirstOrDefault(v => v.Name == procedure.VetName);

                if (!IsValid(procedure) ||
                    animal == null ||
                    vet == null ||
                    procedure.AnimalAids == null ||
                    procedure.AnimalAids.Any(aa => aa.Name == null))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                AnimalAid[] animalAids = procedure.AnimalAids
                    .Select(aa => animalAidByName[aa.Name])
                    .ToArray();

                bool animalAidsExist = animalAids
                    .All(animalAid => animalAid != null);
                
                //bool hasSameAnimalAidMoreThanOnce = procedure.AnimalAids
                //    .Select(aa => aa.Name)
                //    .Distinct()
                //    .Count() != procedure.AnimalAids.Length;

                bool hasSameAnimalAidMoreThanOnce = procedure.AnimalAids
                    .GroupBy(aa => aa.Name)
                    .Count() != procedure.AnimalAids.Length;

                if (!animalAidsExist || hasSameAnimalAidMoreThanOnce)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                proceduresToAdd.Add(new Procedure
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = procedure.DateTime,
                    ProcedureAnimalAids = animalAids
                        .Select(animalAid => new ProcedureAnimalAid
                        {
                            AnimalAid = animalAid
                        })
                        .ToArray()
                });

                result.AppendLine("Record successfully imported.");
            }

            context.Procedures.AddRange(proceduresToAdd);
            
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }
    }
}