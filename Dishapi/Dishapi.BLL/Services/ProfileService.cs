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

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return null ;

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto profileDto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var existingProfile = await _context.Profiles
                .AnyAsync(p => p.UserId == userId);

            if (existingProfile)
                throw new InvalidOperationException("Profile already exists for this user");

            var profile = new Profile
            {
                UserId = userId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                PhoneNumber = profileDto.PhoneNumber,
                Address = profileDto.Address,
               
                CreatedAt = DateTime.UtcNow
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<ProfileResponseDto> UpdateProfileAsync(string userId, ProfileUpdateDto profileDto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                throw new KeyNotFoundException("Profile not found");

            profile.FirstName = profileDto.FirstName;
            profile.LastName = profileDto.LastName;
            profile.PhoneNumber = profileDto.PhoneNumber;
            profile.Address = profileDto.Address;
            profile.City = profileDto.City;
            profile.Country = profileDto.Country;
            profile.PostalCode = profileDto.PostalCode;
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponseDto(profile);
        }

        public async Task<bool> DeleteProfileAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProfileExistsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }

        private ProfileResponseDto MapToResponseDto(Profile profile)
        {
            return new ProfileResponseDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                City = profile.City,
                Country = profile.Country,
                PostalCode = profile.PostalCode,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };
        }
    }
}