using System;
using System.Collections.Generic;
using System.Linq;

using Stations.Models;

namespace Stations.Data
{
    public static class DataManager
    {
        public static SeatingClass SeatingClassByNameAndAbbreviation(StationsDbContext db, string name, string abbreviation)
            => db.SeatingClasses
                .FirstOrDefault(sc => sc.Name == name && 
                    sc.Abbreviation == abbreviation);

        public static void SaveTrains(StationsDbContext db, IEnumerable<Train> trainsToAdd)
        {
            db.Trains.AddRange(trainsToAdd);

            db.SaveChanges();
        }

        public static void SaveSeatingClasses(StationsDbContext db, IEnumerable<SeatingClass> seatingClassesToAdd)
        {
            db.SeatingClasses.AddRange(seatingClassesToAdd);

            db.SaveChanges();
        }

        public static void SaveStations(StationsDbContext db, IEnumerable<Station> stationsToAdd)
        {
            db.Stations.AddRange(stationsToAdd);

            db.SaveChanges();
        }

        public static Train TrainByNumber(StationsDbContext db, string trainNumber)
            => db.Trains.FirstOrDefault(t => t.TrainNumber == trainNumber);

        public static Station StationByName(StationsDbContext db, string stationName)
            => db.Stations.FirstOrDefault(s => s.Name == stationName);

        public static void SaveTrips(StationsDbContext db, IEnumerable<Trip> tripsToAdd)
        {
            db.Trips.AddRange(tripsToAdd);

            db.SaveChanges();
        }

        public static void SaveCards(StationsDbContext db, IEnumerable<CustomerCard> cardsToAdd)
        {
            db.CustomerCards.AddRange(cardsToAdd);

            db.SaveChanges();
        }

        public static Trip TripByStationsAndDepartureTime(StationsDbContext db, string originStation, string destinationStation, DateTime departureTime)
            => db.Trips
                .FirstOrDefault(t => t.OriginStation.Name == originStation &&
                    t.DestinationStation.Name == destinationStation &&
                    t.DepartureTime == departureTime);

        public static CustomerCard CardByName(StationsDbContext db, string cardName)
            => db.CustomerCards.FirstOrDefault(c => c.Name == cardName);

        public static SeatingClass SeatingClassByAbbreviation(StationsDbContext db, string abbreviation)
            => db.SeatingClasses.FirstOrDefault(sc => sc.Abbreviation == abbreviation);

        public static void SaveTickets(StationsDbContext db, IEnumerable<Ticket> ticketsToAdd)
        {
            db.Tickets.AddRange(ticketsToAdd);

            db.SaveChanges();
        }
    }
}