namespace Dishapi.Models
{
    public class ProfileResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
    }
}
