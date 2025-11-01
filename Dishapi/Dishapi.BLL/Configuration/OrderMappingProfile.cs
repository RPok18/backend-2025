// File: Dishapi.BLL/Configuration/OrderMappingProfile.cs
using AutoMapper;
using Dishapi.Core.Dtos;
// Remove: using Dishapi.DTOs;

namespace Dishapi.BLL.Configuration
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            // Map Order entity to OrderDto
            CreateMap<Dishapi.DAL.Entities.Order, OrderDto>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            // Map OrderItem entity to OrderItemDto
            CreateMap<Dishapi.DAL.Entities.OrderItem, OrderItemDto>();
        }
    }
}