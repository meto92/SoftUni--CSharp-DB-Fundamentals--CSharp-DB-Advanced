using System.Linq;
using AutoMapper;
using Stations.DTOs.Import;
using Stations.Models;

namespace Stations.App
{
    public class StationsProfile : Profile
    {
        public StationsProfile()
        {
            CreateMap<StationDto, Station>()
                .ForMember(dto => dto.Town,
                    opt => opt.MapFrom(src => src.Town ?? src.Name));

            CreateMap<CustomerCard, DTOs.Export.CardDto>()
                .ForMember(dto => dto.Tickets,
                    opt => opt.MapFrom(src => src.Tickets
                        .Select(t => new DTOs.Export.TicketDto
                        {
                            OriginStation = t.Trip.OriginStation.Name,
                            DestinationStation = t.Trip.DestinationStation.Name,
                            DepartureTime = t.Trip.DepartureTime
                        })));
        }
    }
}