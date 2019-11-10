using System.Linq;

using AutoMapper;

using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.Client
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<Product, ProductWithBuyerDto>()
                .ForMember(dto => dto.Buyer,
                    opt => opt.MapFrom(src => src.Buyer.FullName));

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.CategoryIds,
                    opt => opt.MapFrom(src => src.ProductCategories
                        .Select(cp => cp.CategoryId)));
        }
    }
}