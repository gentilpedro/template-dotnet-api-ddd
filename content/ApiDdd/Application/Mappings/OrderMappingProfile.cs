using AutoMapper;
using DddApiTemplate.Application.DTOs;
using DddApiTemplate.Domain.Entities;

namespace DddApiTemplate.Application.Mappings;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForCtorParam(nameof(OrderItemDto.UnitPrice), opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForCtorParam(nameof(OrderItemDto.Total), opt => opt.MapFrom(src => src.Total.Amount));

        CreateMap<Order, OrderDto>()
            .ForCtorParam(nameof(OrderDto.Status), opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(OrderDto.Total), opt => opt.MapFrom(src => src.Total.Amount))
            .ForCtorParam(nameof(OrderDto.Items), opt => opt.MapFrom(src => src.Items));
    }
}
