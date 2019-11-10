using System.Linq;

using AutoMapper;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.Models;

namespace TeamBuilder.App.Configuration
{
    public class TeamBuilderProfile : Profile
    {
        public TeamBuilderProfile()
        {
            CreateMap<User, User>();

            CreateMap<Team, Team>();

            CreateMap<Event, Event>();

            CreateMap<Event, EventWithTeamsDto>()
                .ForMember(dto => dto.ParticipatingTeamNames,
                    opt => opt.MapFrom(src => src.ParticipatingEventTeams
                        .Select(et => et.Team.Name)));

            CreateMap<Team, TeamWithMembersDto>()
                .ForMember(dto => dto.Members,
                    opt => opt.MapFrom(src => src.TeamUsers
                        .Select(ut => ut.User.Username)));
        }
    }
}