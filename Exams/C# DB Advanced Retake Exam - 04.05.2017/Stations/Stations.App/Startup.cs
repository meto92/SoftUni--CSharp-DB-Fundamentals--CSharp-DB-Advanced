using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using Stations.Data;
using Stations.DTOs.Import;
using Stations.Export;
using Stations.Import;
using Stations.Models.Enums;

namespace Stations.App
{
    public class Startup
    {
        private const string ResourcesDirectory = "Resources";

        private static void ImportData(StationsDbContext db)
        {
            //string stationsJson = File.ReadAllText($@"{ResourcesDirectory}\stations.json");
            //string importStationsResult = Deserializer.ImportStations(db, stationsJson);

            //Console.WriteLine(importStationsResult);

            //string seatingClassesJson = File.ReadAllText($@"{ResourcesDirectory}\classes.json");
            //string importSeatingClassesResult = Deserializer.ImportSeatingClasses(db, seatingClassesJson);

            //Console.WriteLine(importSeatingClassesResult);

            string trainsJson = File.ReadAllText($@"{ResourcesDirectory}\trains.json");
            string importTrainsResult = Deserializer.ImportTrains(db, trainsJson);

            //Console.WriteLine(importTrainsResult);

            //string tripsJson = File.ReadAllText($@"{ResourcesDirectory}\trips.json");
            //string importTripsResult = Deserializer.ImportTrips(db, tripsJson);

            //Console.WriteLine(importTripsResult);

            //string cardsXml = File.ReadAllText($@"{ResourcesDirectory}\cards.xml");
            //string importCardsResult = Deserializer.ImportCards(db, cardsXml);

            //Console.WriteLine(importCardsResult);

            //string ticketsXml = File.ReadAllText($@"{ResourcesDirectory}\tickets.xml");
            //string importTicketsResult = Deserializer.ImportTickets(db, ticketsXml);

            //Console.WriteLine(importTicketsResult);
        }

        private static void ExportData(StationsDbContext db)
        {
            //Serializer.ExportDelayedTrains(db, "01/01/2017");

            Serializer.ExportCardsByType(db, "Elder");
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<StationsProfile>());

            using (StationsDbContext db = new StationsDbContext())
            {
                //return;
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                //ImportData(db);
                ExportData(db);
            }
        }
    }
}