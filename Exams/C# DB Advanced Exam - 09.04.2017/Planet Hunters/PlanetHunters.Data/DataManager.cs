using System.Collections.Generic;
using System.Linq;

using PlanetHunters.Models;

namespace PlanetHunters.Data
{
    public static class DataManager
    {
        public static StarSystem StarSystemByName(PlanetHuntersDbContext db, string starSystemName)
            => db.StarSystems.FirstOrDefault(ss => ss.Name == starSystemName);

        public static Star StarByName(PlanetHuntersDbContext db, string starName)
            => db.Stars.FirstOrDefault(s => s.Name == starName);

        public static Planet PlanetByName(PlanetHuntersDbContext db, string planetName)
            => db.Planets.FirstOrDefault(p => p.Name == planetName);

        public static Astronomer AstronomerByNames(PlanetHuntersDbContext db, string firstName, string lastName)
            => db.Astronomers
                .FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);

        public static Telescope TelescopeByName(PlanetHuntersDbContext db, string telescopeName)
            => db.Telescopes.FirstOrDefault(t => t.Name == telescopeName);

        public static void SaveAstronomers(PlanetHuntersDbContext db, IEnumerable<Astronomer> astronomersToAdd)
        {
            db.Astronomers.AddRange(astronomersToAdd);

            db.SaveChanges();
        }

        public static void SaveTelescopes(PlanetHuntersDbContext db, IEnumerable<Telescope> telescopesToAdd)
        {
            db.Telescopes.AddRange(telescopesToAdd);

            db.SaveChanges();
        }

        public static void SavePlanets(PlanetHuntersDbContext db, IEnumerable<Planet> planetsToAdd)
        {
            db.Planets.AddRange(planetsToAdd);

            db.SaveChanges();
        }

        public static void SaveStars(PlanetHuntersDbContext db, IEnumerable<Star> starsToAdd)
        {
            db.Stars.AddRange(starsToAdd);

            db.SaveChanges();
        }

        public static void SaveDiscoveries(PlanetHuntersDbContext db, IEnumerable<Discovery> discoveriesToAdd)
        {
            db.Discoveries.AddRange(discoveriesToAdd);

            db.SaveChanges();
        }
    }
}