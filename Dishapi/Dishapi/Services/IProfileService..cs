using Dishapi.Models;

namespace Dishapi.Services
{
    public interface IProfileService
    {
        Task<Profile> GetProfileAsync(string userId);
        Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId);
        Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto dto);
        Task<ProfileResponseDto?> UpdateProfileAsync(string id, ProfileUpdateDto dto);
        Task<bool> DeleteProfileAsync(string id);
        Task<bool> ProfileExistsAsync(string id);
    }
}