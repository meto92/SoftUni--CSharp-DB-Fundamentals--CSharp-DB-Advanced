using AutoMapper;

using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Models;

namespace BusTicketsSystem.Client.Core
{
    public class BusTicketsSystemProfile : Profile
    {
        public BusTicketsSystemProfile()
        {
            CreateMap<BusCompany, BusCompany>();

            CreateMap<BusStation, BusStation>();

            CreateMap<Customer, Customer>();

            CreateMap<Review, Review>();

            CreateMap<Ticket, Ticket>();

            CreateMap<Trip, Trip>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(dto => dto.Balance,
                    opt => opt.MapFrom(src => src.BankAccount.Balance))
                .ForMember(dto => dto.BankAccountNumber,
                    opt => opt.MapFrom(src => src.BankAccount.AccountNumber));

            CreateMap<BusCompany, BusCompanyDto>();

            CreateMap<BusStation, BusStationDto>()
                .ForMember(dto => dto.Town,
                    opt => opt.MapFrom(src => src.Town.Name));

            CreateMap<Review, ReviewDto>();

            CreateMap<Ticket, TicketDto>();

            CreateMap<Trip, TripDto>();

            CreateMap<Trip, TripChangeStatusDto>()
                .ForMember(dto => dto.OriginBusStationTown,
                    opt => opt.MapFrom(src => src.OriginBusStation.Town.Name))
                .ForMember(dto => dto.DestinationBusStationTown,
                    opt => opt.MapFrom(src => src.DestinationBusStation.Town.Name))
                .ForMember(dto => dto.PassengersCount,
                    opt => opt.MapFrom(src => src.Tickets.Count));

            CreateMap<Trip, ArrivalDto>()
                .ForMember(dto => dto.OriginBusStationTown,
                    opt => opt.MapFrom(src => src.OriginBusStation.Town.Name))
                .ForMember(dto => dto.ArrivalHour,
                    opt => opt.MapFrom(src => src.ArrivalTime.Hour))
                .ForMember(dto => dto.ArrivalMinutes,
                    opt => opt.MapFrom(src => src.ArrivalTime.Minute));

            CreateMap<Trip, DepartureDto>()
                .ForMember(dto => dto.DestinationBusStationTown,
                    opt => opt.MapFrom(src => src.DestinationBusStation.Town.Name))
                .ForMember(dto => dto.DepartureHour,
                    opt => opt.MapFrom(src => src.DepartureTime.Hour))
                .ForMember(dto => dto.DepartureMinutes,
                    opt => opt.MapFrom(src => src.DepartureTime.Minute));
        }
    }
}