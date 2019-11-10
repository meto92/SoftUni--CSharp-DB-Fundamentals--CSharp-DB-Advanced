namespace PetClinic.App
{
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using PetClinic.DTOs;
    using PetClinic.Models;

    public class PetClinicProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public PetClinicProfile()
        {
            CreateMap<Animal, ExportAnimalDto>()
                .ForMember(dto => dto.OwnerName,
                    opt => opt.MapFrom(src => src.Passport.OwnerName))
                .ForMember(dto => dto.RegisteredOn,
                    opt => opt.MapFrom(src => src.Passport.RegistrationDate.ToString("dd-MM-yyyy")));
            
            CreateMap<Procedure, ExportProcedureDto>()
                .ForMember(dto => dto.Passport,
                    opt => opt.MapFrom(src => src.Animal.PassportSerialNumber))
                .ForMember(dto => dto.OwnerNumber,
                    opt => opt.MapFrom(src => src.Animal.Passport.OwnerPhoneNumber))
                .ForMember(dto => dto.DateTime,
                    opt => opt.MapFrom(src => src.DateTime
                        .ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dto => dto.AnimalAids,
                    opt => opt.MapFrom(src => src.ProcedureAnimalAids
                        .Select(paa => paa.AnimalAid)))
                .ForMember(dto => dto.TotalPrice,
                    opt => opt.MapFrom(src => src
                        .ProcedureAnimalAids.Sum(paa => paa.AnimalAid.Price)));
        }
    }
}