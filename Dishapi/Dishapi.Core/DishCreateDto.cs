using System.ComponentModel.DataAnnotations;

namespace Dishapi.Core.Dtos
{
    public class DishCreateDto
    {
        [Required]
        public int ProfileId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}