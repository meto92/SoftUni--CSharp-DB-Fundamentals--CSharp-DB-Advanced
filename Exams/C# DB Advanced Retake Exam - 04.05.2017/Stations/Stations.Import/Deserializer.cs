using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AutoMapper.QueryableExtensions;

using Newtonsoft.Json;

using Stations.Data;
using Stations.DTOs.Import;
using Stations.Models;
using Stations.Models.Enums;

namespace Stations.Import
{
    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";
        private const string TripSuccessMessage = "Trip from {0} to {1} imported.";
        private const string TicketSuccessMessage = "Ticket from {0} to {1} departing at {2} imported.";

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        public static string ImportStations(StationsDbContext db, string stationsJson)
        {
            StationDto[] stations = JsonConvert.DeserializeObject<StationDto[]>(stationsJson);

            StringBuilder result = new StringBuilder();

            List<StationDto> validStations = new List<StationDto>();

            foreach (StationDto station in stations)
            {
                bool isValid = IsValid(station);

                if (!isValid || validStations.Any(s => s.Name == station.Name))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validStations.Add(station);

                result.AppendLine(string.Format(SuccessMessage, station.Name));
            }

            IEnumerable<Station> stationsToAdd = validStations
                .AsQueryable()
                .ProjectTo<Station>();

            DataManager.SaveStations(db, stationsToAdd);

            return result.ToString().TrimEnd();
        }
        
        public static string ImportSeatingClasses(StationsDbContext db, string seatingClassesJson)
        {
            SeatingClassDto[] seatingClasses = JsonConvert.DeserializeObject<SeatingClassDto[]>(seatingClassesJson);

            StringBuilder result = new StringBuilder();

            List<SeatingClassDto> validClasses = new List<SeatingClassDto>();

            foreach (SeatingClassDto seatingClass in seatingClasses)
            {
                bool isValid = IsValid(seatingClass);

                if (!isValid || validClasses.Any(sc => sc.Name == seatingClass.Name))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validClasses.Add(seatingClass);

                result.AppendLine(string.Format(SuccessMessage, seatingClass.Name));
            }

            IEnumerable<SeatingClass> seatingClassesToAdd = validClasses
                .AsQueryable()
                .ProjectTo<SeatingClass>();

            DataManager.SaveSeatingClasses(db, seatingClassesToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportTrains(StationsDbContext db, string trainsJson)
        {
            TrainDto[] trains = JsonConvert.DeserializeObject<TrainDto[]>(trainsJson);

            StringBuilder result = new StringBuilder();

            List<TrainDto> validTrains = new List<TrainDto>();

            foreach (TrainDto train in trains)
            {
                bool isValid = IsValid(train);

                if (isValid && train.Seats != null && train.Seats.Any(sc => !IsValid(sc)))
                {
                    isValid = false;
                }

                if (!isValid ||
                    validTrains.Any(t => t.TrainNumber == train.TrainNumber))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                if (train.Seats != null)
                {
                    bool seatingClassesExist = train.Seats
                    .All(ss => DataManager
                        .SeatingClassByNameAndAbbreviation(db, ss.Name, ss.Abbreviation) != null);

                    if (!seatingClassesExist)
                    {
                        result.AppendLine(ErrorMessage);

                        continue;
                    }
                }

                validTrains.Add(train);

                result.AppendLine(string.Format(SuccessMessage, train.TrainNumber));
            }

            Train[] trainsToAdd = validTrains
                .Select(t => new Train
                {
                    TrainNumber = t.TrainNumber,
                    Type = t.Type == null 
                        ? null 
                        : (TrainType?) Enum.Parse<TrainType>(t.Type),
                    Seats = t.Seats?
                        .Select(s => new TrainSeats
                        {
                            SeatingClass = DataManager
                                .SeatingClassByNameAndAbbreviation(db, s.Name, s.Abbreviation),
                            Quantity = s.Quantity.Value
                        })
                        .ToArray()
                })
                .ToArray();

            DataManager.SaveTrains(db, trainsToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportTrips(StationsDbContext db, string tripsJson)
        {
            TripDto[] trips = JsonConvert.DeserializeObject<TripDto[]>(tripsJson);

            const string dateFormat = "dd/MM/yyyy HH:mm";

            StringBuilder result = new StringBuilder();

            List<TripDto> validTrips = new List<TripDto>();

            foreach (TripDto trip in trips)
            {
                bool isValid = IsValid(trip);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                bool trainExists = DataManager.TrainByNumber(db, trip.Train) != null;

                bool originStationExists = DataManager.StationByName(db, trip.OriginStation) != null;

                bool destinationStationExists = DataManager.StationByName(db, trip.DestinationStation) != null;

                DateTime departureTime = DateTime.ParseExact(trip.DepartureTimeStr, dateFormat, CultureInfo.InvariantCulture);

                DateTime arrivalTime = DateTime.ParseExact(trip.ArrivalTimeStr, dateFormat, CultureInfo.InvariantCulture);

                if (!trainExists ||
                    !originStationExists ||
                    !destinationStationExists ||
                    departureTime > arrivalTime)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validTrips.Add(trip);

                result.AppendLine(string.Format(
                    TripSuccessMessage,
                    trip.OriginStation, trip.DestinationStation));
            }

            Dictionary<string, Station> stationByName = validTrips
                .Select(t => t.OriginStation)
                .Concat(validTrips.Select(t => t.DestinationStation))
                .Distinct()
                .ToDictionary(s => s, s => DataManager.StationByName(db, s));

            Dictionary<string, Train> trainByNumber = validTrips
                .Select(t => t.Train)
                .Distinct()
                .ToDictionary(t => t, t => DataManager.TrainByNumber(db, t));

            Trip[] tripsToAdd = validTrips
                .Select(t => new Trip
                {
                    OriginStation = stationByName[t.OriginStation],
                    DestinationStation = stationByName[t.DestinationStation],
                    ArrivalTime = t.ArrivalTime,
                    DepartureTime = t.DepartureTime,
                    Train = trainByNumber[t.Train],
                    TimeDifference = t.TimeDifference == null 
                        ? null 
                        : (TimeSpan?) TimeSpan.ParseExact(t.TimeDifference, "hh\\:mm", null),
                    Status = t.Status == null 
                        ? TripStatus.OnTime 
                        : Enum.Parse<TripStatus>(t.Status)
                })
                .ToArray();

            DataManager.SaveTrips(db, tripsToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportCards(StationsDbContext db, string cardsXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Cards");
            XmlSerializer serializer = new XmlSerializer(typeof(CardDto[]), root);

            CardDto[] cards = null;

            using (StringReader reader = new StringReader(cardsXml))
            {
                cards = (CardDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            List<CardDto> validCards = new List<CardDto>();

            foreach (CardDto card in cards)
            {
                bool isValid = IsValid(card);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validCards.Add(card);

                result.AppendLine(string.Format(SuccessMessage, card.Name));
            }

            CustomerCard[] cardsToAdd = validCards
                .Select(c => new CustomerCard
                {
                    Name = c.Name,
                    Age = c.Age,
                    Type = c.CardType == null 
                        ? CardType.Normal 
                        : Enum.Parse<CardType>(c.CardType)
                })
                .ToArray();

            DataManager.SaveCards(db, cardsToAdd);

            return result.ToString().TrimEnd();
        }

        public static string ImportTickets(StationsDbContext db, string ticketsXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Tickets");
            XmlSerializer serializer = new XmlSerializer(typeof(TicketDto[]), root);

            TicketDto[] tickets = null;

            using (StringReader reader = new StringReader(ticketsXml))
            {
                tickets = (TicketDto[]) serializer.Deserialize(reader);
            }

            List<TicketDto> validTickets = new List<TicketDto>();

            StringBuilder result = new StringBuilder();

            foreach (TicketDto ticket in tickets)
            {
                if (ticket.Card != null)
                {
                    bool cardExists = DataManager.CardByName(db, ticket.Card.Name) != null;

                    if (!cardExists)
                    {
                        result.AppendLine(ErrorMessage);

                        continue;
                    }
                }

                Trip trip = DataManager.TripByStationsAndDepartureTime(
                    db, 
                    ticket.Trip.OriginStation, 
                    ticket.Trip.DestinationStation, 
                    ticket.Trip.DepartureTime);

                if (trip == null || ticket.Seat.Length <= 2)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                SeatingClass seatingClass = DataManager.SeatingClassByAbbreviation(db, ticket.Seat.Substring(0, 2));

                if (seatingClass == null || 
                    int.TryParse(ticket.Seat.Substring(2), out int seatNumber) ||
                    seatNumber < 0 ||
                    !trip.Train.Seats.Any(ts => ts.SeatingClassId == seatingClass.Id) ||
                    seatNumber > trip.Train.Seats.Sum(ts => ts.Quantity))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validTickets.Add(ticket);

                result.AppendLine(string.Format(
                    TicketSuccessMessage,
                    ticket.Trip.OriginStation,
                    ticket.Trip.DestinationStation,
                    ticket.Trip.DepartureTimeStr));
            }

            Ticket[] ticketsToAdd = validTickets
                .Select(t => new Ticket
                {
                    Price = t.Price,
                    Trip = DataManager.TripByStationsAndDepartureTime(db, t.Trip.OriginStation, t.Trip.DestinationStation, t.Trip.DepartureTime),
                    PersonalCard = t.Card == null ? null : DataManager.CardByName(db, t.Card.Name),
                    SeatingPlace = t.Seat
                })
                .ToArray();

            DataManager.SaveTickets(db, ticketsToAdd);

            return result.ToString().TrimEnd();
        }
    }
}