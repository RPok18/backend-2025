using Dishapi.Data;
using Dishapi.Models;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetProfileAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                throw new KeyNotFoundException($"Profile not found for user {userId}");

            return profile;
        }

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return profile == null ? null : MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto dto)
        {
            var profile = new Profile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                FullName = dto.FullName, // Uses the computed property
                Address = dto.Address,
                Phone = dto.Phone
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string id, ProfileUpdateDto dto)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return null;

            // Update only non-null properties
            if (dto.FullName != null)
                profile.FullName = dto.FullName;

            if (dto.Bio != null)
                profile.Bio = dto.Bio;

            if (dto.BirthDate.HasValue)
                profile.BirthDate = dto.BirthDate.Value;

            if (dto.Address != null)
                profile.Address = dto.Address;

            if (dto.Phone != null)
                profile.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<bool> DeleteProfileAsync(string id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProfileExistsAsync(string id)
        {
            return await _context.Profiles.AnyAsync(p => p.Id == id);
        }

        // Helper method to map Profile to ProfileResponseDto
        private ProfileResponseDto MapToResponseDto(Profile profile)
        {
            return new ProfileResponseDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Bio = profile.Bio,
                BirthDate = profile.BirthDate,
                Address = profile.Address,
                Phone = profile.Phone
            };
        }
    }
}