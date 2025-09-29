using System.ComponentModel.DataAnnotations;

namespace Dishapi.Models
{
    public class Class1
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Links to authenticated user

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
        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Computed property
        public int Age => DateTime.Now.Year - BirthDate.Year -
            (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
    }
}