using Dishapi.Core.Dtos;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Dishapi.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProfileService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Dishapi.DAL.Entities.Profile> GetProfileAsync(string userId)
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

            return profile == null ? null : _mapper.Map<ProfileResponseDto>(profile);
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

            var profile = _mapper.Map<Dishapi.DAL.Entities.Profile>(dto);
            profile.UserId = userIdInt;
            profile.CreatedAt = DateTime.UtcNow;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProfileResponseDto>(profile);
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string id, ProfileUpdateDto dto)
        {
            if (!int.TryParse(id, out var userIdInt))
                throw new ArgumentException("Invalid user id");
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userIdInt);
            if (profile == null) return null;

            _mapper.Map(dto, profile);

            await _context.SaveChangesAsync();
            return _mapper.Map<ProfileResponseDto>(profile);
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

        
    }
}

