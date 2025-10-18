using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dishapi.DAL.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DishId { get; set; }

        [ForeignKey(nameof(DishId))]
        public virtual Dish Dish { get; set; } = null!;

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}