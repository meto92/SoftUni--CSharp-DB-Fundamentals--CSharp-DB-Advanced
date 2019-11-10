using System.Linq;

using AutoMapper;

using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.Client
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<Car, CarWithPartsDto>()
                .ForMember(dto => dto.Parts,
                    opt => opt.MapFrom(src => 
                        src.CarParts.Select(pc => pc.Part)));

            CreateMap<Sale, SaleDto>()
                .ForMember(dto => dto.Discount,
                    opt => opt.MapFrom(src => src.Discount / 100.0))
                .ForMember(dto => dto.IsCustomerYoungDriver,
                    opt => opt.MapFrom(src => src.Customer.IsYoungDriver));

            //CreateMap<Sale, SaleDto>()
            //    .ForMember(dto => dto.Discount,
            //        opt => opt.MapFrom(src => src.Discount / 100.0f))
            //    .ForMember(dto => dto.Price,
            //        opt => opt.MapFrom(src => src.Car.Price))
            //    .ForMember(dto => dto.PriceWithDiscount,
            //        opt => opt.MapFrom(src => (1 - (src.Customer.IsYoungDriver ?  src.Discount + 5 : src.Discount) / 100.0m) * src.Car.Price));
        }
    }
}