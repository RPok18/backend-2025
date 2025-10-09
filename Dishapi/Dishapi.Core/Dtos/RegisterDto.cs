using System.ComponentModel.DataAnnotations;

namespace Dishapi.Core.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

