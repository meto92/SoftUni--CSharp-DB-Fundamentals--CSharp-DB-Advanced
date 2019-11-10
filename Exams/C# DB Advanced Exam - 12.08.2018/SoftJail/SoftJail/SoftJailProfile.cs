namespace SoftJail
{
    using System.Linq;
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            //CreateMap<DepartmentDto, Department>()
            //    .ForMember(dest => dest.Cells,
            //        opt => opt.MapFrom(src => src.Cells
            //            .Select(c => new Cell
            //            {
            //                CellNumber = c.CellNumber,
            //                HasWindow = c.HasWindow
            //            })));

            CreateMap<PrisonerDto, Prisoner>()
                .ForMember(dest => dest.ReleaseDate,
                    opt => opt.Ignore());
        }
    }
}