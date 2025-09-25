using Microsoft.EntityFrameworkCore;
using Profileapi.Data;
using Profileapi.Models;
using Profileapi.Models.DTOs;

namespace Profileapi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ProfileDbContext _context;

        public ProfileService(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return null;

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto profileDto)
        {
            if (await ProfileExistsAsync(userId))
            {
                throw new InvalidOperationException("Profile already exists for this user.");
            }

            var profile = new Profile
            {
                UserId = userId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                BirthDate = profileDto.BirthDate,
                Address = profileDto.Address,
                Phone = profileDto.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string userId, ProfileUpdateDto profileDto)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return null;

            if (!string.IsNullOrEmpty(profileDto.FirstName))
                profile.FirstName = profileDto.FirstName;

            if (!string.IsNullOrEmpty(profileDto.LastName))
                profile.LastName = profileDto.LastName;

            if (profileDto.BirthDate.HasValue)
                profile.BirthDate = profileDto.BirthDate.Value;

            if (!string.IsNullOrEmpty(profileDto.Address))
                profile.Address = profileDto.Address;

            if (!string.IsNullOrEmpty(profileDto.Phone))
                profile.Phone = profileDto.Phone;

            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToResponseDto(profile);
        }

        public async Task<bool> DeleteProfileAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProfileExistsAsync(string userId)
        {
            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }

        private static ProfileResponseDto MapToResponseDto(Profile profile)
        {
            return new ProfileResponseDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate,
                Age = profile.Age,
                Address = profile.Address,
                Phone = profile.Phone,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };
        }
    }
}