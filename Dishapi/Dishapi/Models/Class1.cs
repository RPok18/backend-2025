using System.ComponentModel.DataAnnotations;

namespace Dishapi.Models
{
    public class Class1
    {
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; } // Links to authenticated user

        [Required]
        [StringLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public required string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500)]
        public required string Address { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public required string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Computed property
        public int Age => DateTime.Now.Year - BirthDate.Year -
            (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
    }
}
