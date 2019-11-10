using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Stations.Data;
using Stations.DTOs.Export;
using Stations.Models.Enums;

namespace Stations.Export
{
    public static class Serializer
    {
        private const string ExportDirectory = "ExportedData";

        public static void ExportDelayedTrains(StationsDbContext db, string dateStr)
        {
            DateTime date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DelayedTrainDto[] delayedTrains = db.Trips
                .Where(trip => trip.Status == TripStatus.Delayed &&
                    trip.DepartureTime <= date)
                .GroupBy(trip => trip.Train.TrainNumber)
                .Select(g => new DelayedTrainDto
                {
                    TrainNumber = g.Key,
                    DelayedTimes = g.Count(),
                    MaxDelayedTime = g.Select(t => t.TimeDifference)
                        .Where(d => d != null)
                        .Select(d => (TimeSpan) d)
                        .Max()
                })
                .OrderByDescending(t => t.DelayedTimes)
                .ThenByDescending(t => t.MaxDelayedTime)
                .ThenBy(t => t.TrainNumber)
                .ToArray();

            string json = JsonConvert.SerializeObject(delayedTrains, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText($@"{ExportDirectory}\delayed-trains-by-{dateStr.Replace('/', '-')}.json", json);
        }

        public static void ExportCardsByType(StationsDbContext db, string cardType)
        {
            CardType type = Enum.Parse<CardType>(cardType);

            CardDto[] cards = db.Tickets.Where(t => t.PersonalCard.Type == type)
                .GroupBy(t => t.PersonalCard.Name)
                .Select(g => new CardDto
                {
                    Name = g.Key,
                    Type = cardType,
                    Tickets = g.Select(t => new TicketDto
                    {
                        OriginStation = t.Trip.OriginStation.Name,
                        DestinationStation = t.Trip.DestinationStation.Name,
                        DepartureTime = t.Trip.DepartureTime
                    })
                    .ToArray()
                })
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Cards");
            XmlSerializer serializer = new XmlSerializer(typeof(CardDto[]), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    new XmlQualifiedName(string.Empty, string.Empty)
                });

            string path = $@"{ExportDirectory}\tickets-bought-with-card-type-{cardType}.xml";

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, cards, namespaces);
            }
        }
    }
}