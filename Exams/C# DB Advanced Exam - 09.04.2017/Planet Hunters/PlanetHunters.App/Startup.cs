using System;
using System.IO;

using AutoMapper;

using PlanetHunters.Data;

namespace PlanetHunters.App
{
    public class Startup
    {
        private const string ResourcesPath = "Resources";

        private static void ImportData(PlanetHuntersDbContext db)
        {
            string astronomersJson = File.ReadAllText(ResourcesPath + @"\astronomers.json");
            string astronomersImportResult = DataImporter.DataImporter.ImportAstronomers(db, astronomersJson);

            Console.WriteLine(astronomersImportResult);

            string telescopesJson = File.ReadAllText(ResourcesPath + @"\telescopes.json");
            string telescopesImportResult = DataImporter.DataImporter.ImportTelescopes(db, telescopesJson);

            Console.WriteLine(telescopesImportResult);

            string planetsJson = File.ReadAllText(ResourcesPath + @"\planets.json");
            string planetsImportResult = DataImporter.DataImporter.ImportPlanets(db, planetsJson);

            Console.WriteLine(planetsImportResult);

            string starsXml = File.ReadAllText(ResourcesPath + @"\stars.xml");
            string starsImportResult = DataImporter.DataImporter.ImportStars(db, starsXml);

            Console.WriteLine(starsImportResult);

            string discoveriesXml = File.ReadAllText(ResourcesPath + @"\discoveries.xml");
            string discoveriesImportResult = DataImporter.DataImporter.ImportDiscoveries(db, discoveriesXml);

            Console.WriteLine(discoveriesImportResult);
        }

        private static void ExportData(PlanetHuntersDbContext db)
        {
            string directory = "ExportedData";

            DataExporter.DataExporter.ExportPlanetsByTelescopeName(db, "TRAPPIST", directory);

            DataExporter.DataExporter.ExportAstronomers(db, "Alpha Centauri", directory);

            DataExporter.DataExporter.ExportDiscoveredStars(db, directory);
        }
         
        static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<PlanetHuntersProfile>());

            using (PlanetHuntersDbContext db = new PlanetHuntersDbContext())
            {
                ExportData(db);
            }
        }
        
    }
}