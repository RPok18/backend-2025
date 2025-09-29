namespace Dishapi.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!; // stores the external user id (sub/nameid)
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        // add other fields (AvatarUrl, Phone, etc)
    }
}
