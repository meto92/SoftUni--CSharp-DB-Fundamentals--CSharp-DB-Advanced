using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AutoMapper.QueryableExtensions;

using Newtonsoft.Json;

using PlanetHunters.Data;
using PlanetHunters.DataImporter.DTOs;
using PlanetHunters.Models;

namespace PlanetHunters.DataImporter
{
    public static class DataImporter
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        public static string ImportAstronomers(PlanetHuntersDbContext db, string astronomersJson)
        {
            AstronomerDto[] astronomers = JsonConvert.DeserializeObject<AstronomerDto[]>(astronomersJson);

            StringBuilder result = new StringBuilder();

            List<AstronomerDto> validAstronomers = new List<AstronomerDto>();

            foreach (AstronomerDto astronomer in astronomers)
            {
                bool isValid = IsValid(astronomer);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validAstronomers.Add(astronomer);

                result.AppendLine(string.Format(
                    SuccessMessage,
                    $"{astronomer.FirstName} {astronomer.LastName}"));
            }

            Astronomer[] astronomersToAdd = validAstronomers
                .AsQueryable()
                .ProjectTo<Astronomer>()
                .ToArray();

            DataManager.SaveAstronomers(db, astronomersToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportTelescopes(PlanetHuntersDbContext db, string telescopesJson)
        {
            TelescopeDto[] telescopes = JsonConvert.DeserializeObject<TelescopeDto[]>(telescopesJson);

            StringBuilder result = new StringBuilder();

            List<TelescopeDto> validTelescopes = new List<TelescopeDto>();

            foreach (TelescopeDto telescope in telescopes)
            {
                bool isValid = IsValid(telescope);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validTelescopes.Add(telescope);

                result.AppendLine(string.Format(
                    SuccessMessage,
                    telescope.Name));
            }

            Telescope[] telescopesToAdd = validTelescopes
                .AsQueryable()
                .ProjectTo<Telescope>()
                .ToArray();

            DataManager.SaveTelescopes(db, telescopesToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportPlanets(PlanetHuntersDbContext db, string planetsJson)
        {
            PlanetDto[] planets = JsonConvert.DeserializeObject<PlanetDto[]>(planetsJson);

            StringBuilder result = new StringBuilder();

            List<PlanetDto> validPlanets = new List<PlanetDto>();

            foreach (PlanetDto planet in planets)
            {
                bool isValid = IsValid(planet);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validPlanets.Add(planet);

                result.AppendLine(string.Format(
                    SuccessMessage,
                    planet.Name));
            }

            Dictionary<string, StarSystem> starSystemByName = validPlanets
                .Select(p => p.StarSystem)
                .Distinct()
                .ToDictionary(ss => ss, ss => new StarSystem { Name = ss });

            IEnumerable<Planet> planetsToAdd = validPlanets
                .Select(p => new Planet
                {
                    Name = p.Name,
                    Mass = p.Mass,
                    HostStarSystem = starSystemByName[p.StarSystem]
                });

            DataManager.SavePlanets(db, planetsToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportStars(PlanetHuntersDbContext db, string starsXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Stars");
            XmlSerializer serializer = new XmlSerializer(typeof(StarDto[]), root);

            StarDto[] stars = null;

            using (StringReader reader = new StringReader(starsXml))
            {
                stars = (StarDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            List<StarDto> validStars = new List<StarDto>();

            foreach (StarDto star in stars)
            {
                bool isValid = IsValid(star);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validStars.Add(star);

                result.AppendLine(string.Format(SuccessMessage, star.Name));
            }

            Dictionary<string, StarSystem> starSystemByName = validStars
                .Select(s => s.StarSystem)
                .Distinct()
                .ToDictionary(ss => ss, ss => 
                    DataManager.StarSystemByName(db, ss) 
                    ?? new StarSystem { Name = ss });

            IEnumerable<Star> starsToAdd = validStars
                .Select(s => new Star
                {
                    Name = s.Name,
                    Temperature = s.Temperature,
                    HostStarSystem = starSystemByName[s.StarSystem]
                });

            DataManager.SaveStars(db, starsToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportDiscoveries(PlanetHuntersDbContext db, string discoveriesXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Discoveries");
            XmlSerializer serializer = new XmlSerializer(typeof(DiscoveryDto[]), root);

            DiscoveryDto[] discoveries = null;

            using (StringReader reader = new StringReader(discoveriesXml))
            {
                discoveries = (DiscoveryDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            List<DiscoveryDto> validDiscoveries = new List<DiscoveryDto>();

            foreach (DiscoveryDto discovery in discoveries)
            {
                bool starsExist = discovery.Stars
                    .All(s => DataManager.StarByName(db, s.StarName) != null);
                
                bool planetsExist = discovery.Planets
                    .All(p => DataManager.PlanetByName(db, p.PlanetName) != null);

                bool pioneersExist = discovery.Pioneers
                    .All(p => DataManager
                        .AstronomerByNames(db, p.FirstName, p.LastName) != null);

                bool observersExist = discovery.Observers
                    .All(o => DataManager
                        .AstronomerByNames(db, o.FirstName, o.LastName) != null);

                if (!starsExist || !planetsExist || !pioneersExist || !observersExist)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }
                
                validDiscoveries.Add(discovery);

                result.AppendLine(string.Format(
                    SuccessMessage,
                    string.Format(
                        "({0}-{1}) with {2} star(s), {3} planet(s), {4} pioneer(s) and {5} observers",
                        discovery.Date,
                        discovery.Telescope,
                        discovery.Stars.Length,
                        discovery.Planets.Length,
                        discovery.Pioneers.Length,
                        discovery.Observers.Length)));
            }

            IEnumerable<Discovery> discoveriesToAdd = validDiscoveries
                .Select(d => new Discovery
                {
                    DateMade = d.DateMade,
                    Telescope = DataManager.TelescopeByName(db, d.Telescope),
                    Planets = d.Planets
                        .Select(p => DataManager.PlanetByName(db, p.PlanetName))
                        .ToArray(),
                    Stars = d.Stars
                        .Select(s => DataManager.StarByName(db, s.StarName))
                        .ToArray(),
                    Astronomers = d.Pioneers
                        .Select(p => new AstronomerDiscovery
                        {
                            Astronomer = DataManager
                                .AstronomerByNames(db, p.FirstName, p.LastName)
                        })
                        .ToArray(),
                    Observers = d.Observers
                        .Select(o => new ObserverDiscovery
                        {
                            Observer = DataManager
                                .AstronomerByNames(db, o.FirstName, o.LastName)
                        })
                        .ToArray()
                });

            DataManager.SaveDiscoveries(db, discoveriesToAdd);

            return result.ToString().TrimEnd();
        }
    }
}