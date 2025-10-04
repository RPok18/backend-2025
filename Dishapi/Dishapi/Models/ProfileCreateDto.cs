namespace Dishapi.Models
{
    public class ProfileCreateDto
    {
        // DTO used for creating/updating profiles.
        // Use FirstName/LastName instead of relying on a single FullName string.
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }

        // Convenience computed property — if your service/code referenced dto.FullName,
        // this will satisfy that usage without breaking the DTO shape.
        public string FullName => $"{FirstName} {LastName}";
    }
}
