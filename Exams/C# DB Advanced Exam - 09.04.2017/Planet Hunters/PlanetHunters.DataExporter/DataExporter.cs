using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using AutoMapper.QueryableExtensions;

using Newtonsoft.Json;

using PlanetHunters.Data;
using PlanetHunters.DataExporter.DTOs;
using PlanetHunters.Models;

namespace PlanetHunters.DataExporter
{
    public static class DataExporter
    {
        private const string TelescopeNotFoundMessage = "Telescope {0} not found!";
        private const string StarSystemNotFoundMessage = "Star system {0} not found!";

        public static void ExportPlanetsByTelescopeName(PlanetHuntersDbContext db, string telescopeName, string directory)
        {
            Telescope telescope = DataManager.TelescopeByName(db, telescopeName);

            if (telescope == null)
            {
                throw new ArgumentException(string.Format(
                    TelescopeNotFoundMessage,
                    telescopeName));
            }

            PlanetDto[] planets = telescope.Discoveries
                .SelectMany(d => d.Planets)
                .AsQueryable()
                .ProjectTo<PlanetDto>()
                .OrderByDescending(p => p.Mass)
                .ToArray();

            string json = JsonConvert.SerializeObject(planets, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText($@"{directory}\planets-by-{telescopeName}.json", json);
        }

        public static void ExportAstronomers(PlanetHuntersDbContext db, string starSystemName, string directory)
        {
            StarSystem starSystem = DataManager.StarSystemByName(db, starSystemName);

            if (starSystem == null)
            {
                throw new ArgumentException(string.Format(
                    TelescopeNotFoundMessage,
                    starSystemName));
            }

            AstronomerDto[] pioneers = db.Stars
                .Where(s => s.HostStarSystemId == starSystem.Id)
                .SelectMany(s => s.Discovery.Astronomers)
                .Concat(db.Planets
                    .Where(p => p.HostStarSystemId == starSystem.Id)
                    .SelectMany(p => p.Discovery.Astronomers))
                .Select(ad => new AstronomerDto
                {
                    FirstName = ad.Astronomer.FirstName,
                    LastName = ad.Astronomer.LastName,
                    Role = "pioneer"
                })
                .ToArray();

            AstronomerDto[] observers = db.Stars
                .Where(s => s.HostStarSystemId == starSystem.Id)
                .SelectMany(s => s.Discovery.Observers)
                .Concat(db.Planets
                    .Where(p => p.HostStarSystemId == starSystem.Id)
                    .SelectMany(p => p.Discovery.Observers))
                .Select(od => new AstronomerDto
                {
                    FirstName = od.Observer.FirstName,
                    LastName = od.Observer.LastName,
                    Role = "observer"
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(pioneers.Concat(observers).OrderBy(a => a.LastName), Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText($@"{directory}\astronomers-of-{starSystemName}.json", json);
        }

        public static void ExportDiscoveredStars(PlanetHuntersDbContext db, string directory)
        {
            StarDto[] discoveredStars = db.Stars
                .Where(s => s.DiscoveryId != null)
                .Select(s => new StarDto
                {
                    Name = s.Name,
                    Temperature = s.Temperature,
                    StarSystem = s.HostStarSystem.Name,
                    DiscoveryInfo = new DiscoveryInfoDto
                    {
                        DiscoveryDate = s.Discovery.DateMade.ToString("yyyy-MM-dd"),
                        TelescopeName = s.Discovery.Telescope.Name
                    }
                })
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Stars");
            XmlSerializer serializer = new XmlSerializer(typeof(StarDto[]), root);
            XmlSerializerNamespaces namespaces =
               new XmlSerializerNamespaces(new[]
               {
                    new XmlQualifiedName(string.Empty, string.Empty)
               });

            string path = $@"{directory}\stars.xml";

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, discoveredStars, namespaces);
            }
        }
    }
}