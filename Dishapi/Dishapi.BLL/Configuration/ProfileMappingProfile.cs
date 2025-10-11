using AutoMapper;
using Dishapi.Core.Dtos;
using Dishapi.DAL.Entities;

namespace Dishapi.BLL.Configuration
{
    public class ProfileMappingProfile : AutoMapper.Profile
    {
        public ProfileMappingProfile()
        {
            CreateMap<Dishapi.DAL.Entities.Profile, ProfileResponseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId.ToString()))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone));

            CreateMap<ProfileCreateDto, Dishapi.DAL.Entities.Profile>()
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());

            CreateMap<ProfileUpdateDto, Dishapi.DAL.Entities.Profile>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}


