using Dishapi.Core.Dtos;

namespace Dishapi.BLL.Services
{
    public interface IProfileService
    {
        Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId);
        Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto profileDto);
        Task<ProfileResponseDto> UpdateProfileAsync(string userId, ProfileUpdateDto profileDto);
        Task<bool> DeleteProfileAsync(string userId);
        Task<bool> ProfileExistsAsync(string userId);
    }
}