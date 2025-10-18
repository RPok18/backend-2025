using System.ComponentModel.DataAnnotations;

namespace Dishapi.Core.Dtos
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class RatingCreateDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5")]
        public int Score { get; set; }

        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
    }

    public class RatingUpdateDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5")]
        public int Score { get; set; }

        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
    }

    public class RatingResponseDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class DishRatingSummaryDto
    {
        public int DishId { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
    }
}