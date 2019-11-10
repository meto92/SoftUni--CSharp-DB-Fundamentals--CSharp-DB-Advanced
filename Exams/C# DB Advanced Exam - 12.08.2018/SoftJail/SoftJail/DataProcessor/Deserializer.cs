namespace SoftJail.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string DateFormat = "dd/MM/yyyy";
        private const string ErrorMessage = "Invalid Data";
        private const string DepartmentSuccessfullyAddedMessage = "Imported {0} with {1} cells";
        private const string PrisonerSuccessfullyImportedMessage = "Imported {0} {1} years old";
        private const string OfficerSuccessfullyImportedMessage = "Imported {0} ({1} prisoners)";

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            DepartmentDto[] deserializedDepartments = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            StringBuilder result = new StringBuilder();

            List<DepartmentDto> validDepartments = new List<DepartmentDto>();

            foreach (DepartmentDto department in deserializedDepartments)
            {
                if (!IsValid(department) ||
                    department.Cells.Any(c => !IsValid(c)))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validDepartments.Add(department);

                result.AppendLine(string.Format(
                    DepartmentSuccessfullyAddedMessage,
                    department.Name,
                    department.Cells.Length));
            }

            context.Departments.AddRange(validDepartments.AsQueryable().ProjectTo<Department>());

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            PrisonerDto[] deserializerPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            List<PrisonerDto> validPrisoners = new List<PrisonerDto>();

            StringBuilder result = new StringBuilder();

            //Dictionary<int, Cell> cellById = deserializerPrisoners
            //    .Select(p => p.CellId)
            //    .Distinct()
            //    .ToDictionary(cellId => cellId, cellId => context.Cells.Find(cellId));

            foreach (PrisonerDto prisoner in deserializerPrisoners)
            {
                if (!IsValid(prisoner) ||
                    //cellById[prisoner.CellId] == null ||
                    prisoner.Mails.Any(m => !IsValid(m)))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validPrisoners.Add(prisoner);

                result.AppendLine(string.Format(
                    PrisonerSuccessfullyImportedMessage,
                    prisoner.FullName,
                    prisoner.Age));
            }

            Prisoner[] prisonersToAdd = validPrisoners
                .AsQueryable()
                .ProjectTo<Prisoner>()
                .ToArray();

            for (int i = 0; i < prisonersToAdd.Length; i++)
            {
                string releaseDate = validPrisoners[i].ReleaseDate;

                if (releaseDate != null)
                {
                    prisonersToAdd[i].ReleaseDate = DateTime.ParseExact(releaseDate, DateFormat, CultureInfo.InvariantCulture);
                }
            }

            context.Prisoners.AddRange(prisonersToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlRootAttribute root = new XmlRootAttribute("Officers");
            XmlSerializer serializer = new XmlSerializer(typeof(OfficerDto[]), root);

            OfficerDto[] deserializedOfficersPrisoners = null;

            using (StringReader reader = new StringReader(xmlString))
            {
                deserializedOfficersPrisoners = (OfficerDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            //Dictionary<int, Prisoner> prisonerById = deserializedOfficersPrisoners
            //    .SelectMany(o => o.Prisoners.Select(p => p.Id))
            //    .Distinct()
            //    .ToDictionary(id => id, id => context.Prisoners.Find(id));

            //Dictionary<int, Department> departmentById = deserializedOfficersPrisoners
            //   .Select(o => o.DepartmentId)
            //   .Distinct()
            //   .ToDictionary(id => id, id => context.Departments.Find(id));

            List<Officer> officersToAdd = new List<Officer>();

            foreach (OfficerDto officer in deserializedOfficersPrisoners)
            {
                if (!IsValid(officer) ||
                    !Enum.TryParse(officer.Position, out Position position) ||
                    !Enum.TryParse(officer.Weapon, out Weapon weapon))
                    //departmentById[officer.DepartmentId] == null ||
                    //officer.Prisoners.Any(p => prisonerById[p.Id] == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                officersToAdd.Add(new Officer
                {
                    FullName = officer.FullName,
                    Salary = officer.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officer.DepartmentId,
                    OfficerPrisoners = officer.Prisoners
                        .Select(op => new OfficerPrisoner
                        {
                            PrisonerId = op.Id
                        })
                        .ToArray()
                });

                result.AppendLine(string.Format(
                    OfficerSuccessfullyImportedMessage,
                    officer.FullName,
                    officer.Prisoners.Length));
            }

            context.Officers.AddRange(officersToAdd);

           context.SaveChanges();

            return result.ToString().TrimEnd();
        }
    }
}