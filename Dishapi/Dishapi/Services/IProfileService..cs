using AutoMapper;
using Dishapi.Core.Dtos;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dishapi.Services
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

        public async Task<ProfileResponseDto?> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return profile == null ? null : _mapper.Map<ProfileResponseDto>(profile);
        }

        public async Task<ProfileResponseDto> CreateProfileAsync(string userId, ProfileCreateDto profileDto)
        {
            var profile = _mapper.Map<Profile>(profileDto);
            profile.UserId = userId;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProfileResponseDto>(profile);
        }

        public async Task<ProfileResponseDto?> UpdateProfileAsync(string userId, ProfileUpdateDto profileDto)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
                return null;

            _mapper.Map(profileDto, profile);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProfileResponseDto>(profile);
        }

        public async Task<bool> DeleteProfileAsync(string userId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
                return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProfileExistsAsync(string userId)
        {
            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }
    }
}
