namespace Dishapi.Models
{
    public class ProfileCreateDto
    {
        
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }

        
        public string FullName => $"{FirstName} {LastName}";
    }
}
