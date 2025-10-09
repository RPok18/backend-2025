using Dishapi.Core.Dtos;
using Dishapi.DAL.Entities;

namespace Dishapi.BLL.Services
{
    public interface IProfileService
    {
        Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId);
        Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto dto);
        Task<ProfileResponseDto?> UpdateProfileAsync(string userId, ProfileUpdateDto dto);
        Task<bool> DeleteProfileAsync(string userId);
        Task<bool> ProfileExistsAsync(string userId);
        Task<Profile> GetProfileAsync(string userId);
    }
}

