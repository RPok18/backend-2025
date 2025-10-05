using Dishapi.Data;
using Dishapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dishapi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if user already exists
            if (await UserExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            // Create user
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create profile linked to user
            var profile = new Profile
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Phone = dto.Phone ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Profile = new ProfileResponseDto
                {
                    Id = profile.Id.ToString(),
                    FullName = profile.FullName,
                    Bio = profile.Bio,
                    BirthDate = profile.BirthDate,
                    Address = profile.Address,
                    Phone = profile.Phone
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Profile = new ProfileResponseDto
                {
                    Id = user.Profile!.Id.ToString(),
                    FullName = user.Profile.FullName,
                    Bio = user.Profile.Bio,
                    BirthDate = user.Profile.BirthDate,
                    Address = user.Profile.Address,
                    Phone = user.Profile.Phone
                }
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}