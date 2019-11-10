using System.Linq;
using AutoMapper;

using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.App
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<Product, ProductWithSellerDto>()
                .ForMember(dto => dto.Seller,
                    opt => opt.MapFrom(src => src.Seller.FullName));

            CreateMap<Product, SoldProductWithBuyerDto>()
                .ForMember(dto => dto.BuyerFirstName,
                    opt => opt.MapFrom(src => src.Buyer.FirstName))
                .ForMember(dto => dto.BuyerLastName,
                    opt => opt.MapFrom(src => src.Buyer.LastName));

            CreateMap<Product, ProductWithCategoryIdsDto>()
                .ForMember(dto => dto.CategoryIds,
                    opt => opt.MapFrom(src => src.CategoryProducts
                        .Select(cp => cp.CategoryId)));
        }
    }
}