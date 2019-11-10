namespace PetClinic.DataProcessor
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DTOs;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            ExportAnimalDto[] animals = context
                .Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .ProjectTo<ExportAnimalDto>()
                .OrderBy(a => a.Age)
                .ThenBy(a => a.PassportSerialNumber)
                .ToArray();

            string json = JsonConvert.SerializeObject(animals, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            ExportProcedureDto[] procedures = context.Procedures
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .ProjectTo<ExportProcedureDto>()
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Procedures");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportProcedureDto[]), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    new XmlQualifiedName(string.Empty, string.Empty)
                });

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, procedures, namespaces);

                return writer.ToString();
            }
        }
    }
}