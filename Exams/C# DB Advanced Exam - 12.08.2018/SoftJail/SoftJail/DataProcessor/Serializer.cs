namespace SoftJail.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        private const string DateFormat = "yyyy-MM-dd";

        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            PrisonerDto[] prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new PrisonerDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .Select(po => new OfficerDto
                        {
                            OfficerName = po.Officer.FullName,
                            Department = po.Officer.Department.Name
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers
                        .Sum(po => po.Officer.Salary)
                })
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            string json = JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonerNames = prisonersNames.Split(',').ToArray();

            Func<string, string> reverse = (str) => new string(str.Reverse().ToArray());

            PrisonerXmlDto[] prisoners = context.Prisoners
                .Where(p => prisonerNames.Contains(p.FullName))
                .Select(p => new PrisonerXmlDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    IncarcerationDate = p.IncarcerationDate
                        .ToString(DateFormat, CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                        .Select(m => new MesagedDto
                        {
                            //Description = new string(m.Description.Reverse().ToArray())
                            Description = reverse(m.Description)
                        })
                        .ToArray()
                })
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Prisoners");
            XmlSerializer serializer = new XmlSerializer(typeof(PrisonerXmlDto[]), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    XmlQualifiedName.Empty
                });

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, prisoners, namespaces);

                return writer.ToString();
            }
        }
    }
}