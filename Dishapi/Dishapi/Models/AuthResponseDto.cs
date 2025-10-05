namespace Dishapi.Models
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public ProfileResponseDto Profile { get; set; } = null!;
    }
}