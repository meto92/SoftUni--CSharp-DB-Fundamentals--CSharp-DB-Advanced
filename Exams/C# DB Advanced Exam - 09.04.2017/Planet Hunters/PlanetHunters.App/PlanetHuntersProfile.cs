using AutoMapper;
using PlanetHunters.DataExporter.DTOs;
using PlanetHunters.Models;

namespace PlanetHunters.App
{
    public class PlanetHuntersProfile : Profile
    {
        public PlanetHuntersProfile()
        {
            CreateMap<Planet, PlanetDto>()
                .ForMember(dto => dto.Orbiting,
                    opt => opt.MapFrom(src => new string[] { src.HostStarSystem.Name}));
        }
    }
}