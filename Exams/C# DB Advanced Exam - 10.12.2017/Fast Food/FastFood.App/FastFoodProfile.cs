using System.Linq;
using AutoMapper;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models;

namespace FastFood.App
{
	public class FastFoodProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public FastFoodProfile()
		{
            CreateMap<Order, OrderDto>()
                .ForMember(dto => dto.Items,
                    opt => opt.MapFrom(src => src.OrderItems
                    .Select(oi => new ItemDto
                    {
                        Name = oi.Item.Name,
                        Price = oi.Item.Price,
                        Quantity = oi.Quantity
                    })))
                .ForMember(dto => dto.TotalPrice,
                    opt => opt.MapFrom(src => src.OrderItems
                        .Sum(oi => oi.Quantity * oi.Item.Price)));
		}
	}
}