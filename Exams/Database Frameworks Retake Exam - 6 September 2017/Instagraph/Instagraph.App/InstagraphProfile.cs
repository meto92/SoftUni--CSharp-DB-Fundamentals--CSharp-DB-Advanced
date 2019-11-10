using AutoMapper;

using Instagraph.DTOs.Export;
using Instagraph.Models;

namespace Instagraph.App
{
    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dto => dto.Picture,
                    opt => opt.MapFrom(src => src.Picture.Path))
                .ForMember(dto => dto.User,
                    opt => opt.MapFrom(src => src.User.Username));
        }
    }
}