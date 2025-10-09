using Dishapi.Core.Dtos;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.BLL.Services
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
            if (!int.TryParse(userId, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userIdInt);

            if (profile == null)
                throw new KeyNotFoundException($"Profile not found for user {userId}");

            return profile;
        }

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            if (!int.TryParse(userId, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userIdInt);

            return profile == null ? null : MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto dto)
        {
            if (!int.TryParse(userId, out var userIdInt))
                throw new ArgumentException("Invalid user id");

            var existing = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userIdInt);
            if (existing != null)
            {
                throw new InvalidOperationException("Profile already exists for this user.");
            }

            var profile = new Profile
            {
                UserId = userIdInt,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FullName = dto.FullName,
                Address = dto.Address ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string id, ProfileUpdateDto dto)
        {
            if (!int.TryParse(id, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userIdInt);
            if (profile == null) return null;

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
            if (!int.TryParse(id, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userIdInt);
            if (profile == null) return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProfileExistsAsync(string id)
        {
            if (!int.TryParse(id, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            return await _context.Profiles.AnyAsync(p => p.UserId == userIdInt);
        }

        private ProfileResponseDto MapToResponseDto(Profile profile)
        {
            return new ProfileResponseDto
            {
                Id = profile.Id.ToString(),
                UserId = profile.UserId.ToString(),
                FullName = profile.FullName,
                Bio = profile.Bio,
                BirthDate = profile.BirthDate,
                Address = profile.Address,
                Phone = profile.Phone
            };
        }
    }
}

