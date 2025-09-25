namespace Profileapi.Models.DTOs
{
    public class ProfileCreateDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }

    public class ProfileUpdateDto
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }

    public class ProfileResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
