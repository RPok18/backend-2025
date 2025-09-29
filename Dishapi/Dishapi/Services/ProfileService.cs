using Microsoft.EntityFrameworkCore;
using Dishapi.Data;
using Dishapi.Models;

namespace Dishapi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _db;
        public ProfileService(AppDbContext db) => _db = db;

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            var p = await _db.Profiles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            if (p == null) return null;
            return new ProfileResponseDto { Id = p.Id, UserId = p.UserId, FullName = p.FullName, Bio = p.Bio };
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto dto)
        {
            if (await _db.Profiles.AnyAsync(x => x.UserId == userId))
                throw new InvalidOperationException("Profile already exists.");

            var entity = new Profile { UserId = userId, FullName = dto.FullName, Bio = dto.Bio };
            _db.Profiles.Add(entity);
            await _db.SaveChangesAsync();

            return new ProfileResponseDto { Id = entity.Id, UserId = entity.UserId, FullName = entity.FullName, Bio = entity.Bio };
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string userId, ProfileUpdateDto dto)
        {
            var p = await _db.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (p == null) return null;
            p.FullName = dto.FullName;
            p.Bio = dto.Bio;
            await _db.SaveChangesAsync();
            return new ProfileResponseDto { Id = p.Id, UserId = p.UserId, FullName = p.FullName, Bio = p.Bio };
        }

        public async Task<bool> DeleteProfileAsync(string userId)
        {
            var p = await _db.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (p == null) return false;
            _db.Profiles.Remove(p);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProfileExistsAsync(string userId)
            => await _db.Profiles.AnyAsync(x => x.UserId == userId);
    }
}
