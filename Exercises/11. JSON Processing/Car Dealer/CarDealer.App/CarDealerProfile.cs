using System.Linq;

using AutoMapper;

using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.App
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<Car, CarWithPartsDto>()
                .ForMember(dto => dto.Car,
                    opt => opt.MapFrom(src => src))
                .ForMember(dto => dto.Parts,
                    opt => opt.MapFrom(src => 
                        src.CarParts.Select(pc => pc.Part)));

            CreateMap<Sale, SaleDto>()
                .ForMember(dto => dto.IsCustomerYoungDriver,
                    opt => opt.MapFrom(src => src.Customer.IsYoungDriver));
        }
    }
}