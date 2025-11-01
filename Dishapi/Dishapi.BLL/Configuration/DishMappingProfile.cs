using AutoMapper;
using Dishapi.Core.Dtos;
using Dishapi.DAL.Entities;

namespace Dishapi.BLL.Configuration
{
    public class DishMappingProfile : AutoMapper.Profile
    {
        public DishMappingProfile()
        {
            CreateMap<Dish, DishDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            CreateMap<DishCreateDto, Dish>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());

            CreateMap<DishUpdateDto, Dish>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}