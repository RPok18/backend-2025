using Dishapi.Core.Dtos;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.BLL.Services
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _context;

        public RatingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RatingResponseDto> CreateRatingAsync(string userId, int dishId, RatingCreateDto ratingDto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);

            if (existingRating != null)
                throw new InvalidOperationException("You have already rated this dish. Use update instead.");

            var rating = new Rating
            {
                UserId = userId,
                DishId = dishId,
                Score = ratingDto.Score,
                Comment = ratingDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            await UpdateDishAverageRatingAsync(dishId);

            return new RatingResponseDto
            {
                Id = rating.Id,
                DishId = rating.DishId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }

        public async Task<RatingResponseDto?> GetUserRatingForDishAsync(string userId, int dishId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);

            if (rating == null)
                return null;

            return new RatingResponseDto
            {
                Id = rating.Id,
                DishId = rating.DishId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }

        public async Task<List<RatingDto>> GetRatingsForDishAsync(int dishId, int page = 1, int pageSize = 10)
        {
            var ratings = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RatingDto
                {
                    Id = r.Id,
                    DishId = r.DishId,
                    UserId = r.UserId,
                    Score = r.Score,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync();

            return ratings;
        }

        public async Task<RatingResponseDto> UpdateRatingAsync(string userId, int ratingId, RatingUpdateDto ratingDto)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);

            if (rating == null)
                throw new KeyNotFoundException("Rating not found");

            if (rating.UserId != userId)
                throw new UnauthorizedAccessException("You can only update your own ratings");

            rating.Score = ratingDto.Score;
            rating.Comment = ratingDto.Comment;
            rating.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await UpdateDishAverageRatingAsync(rating.DishId);

            return new RatingResponseDto
            {
                Id = rating.Id,
                DishId = rating.DishId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }

        public async Task<bool> DeleteRatingAsync(string userId, int ratingId)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);

            if (rating == null)
                return false;

            if (rating.UserId != userId)
                throw new UnauthorizedAccessException("You can only delete your own ratings");

            var dishId = rating.DishId;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            await UpdateDishAverageRatingAsync(dishId);

            return true;
        }

        public async Task<DishRatingSummaryDto> GetDishRatingSummaryAsync(int dishId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .ToListAsync();

            if (!ratings.Any())
            {
                return new DishRatingSummaryDto
                {
                    DishId = dishId,
                    AverageRating = 0,
                    TotalRatings = 0,
                    RatingDistribution = new Dictionary<int, int>
                    {
                        { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
                    }
                };
            }

            var distribution = ratings
                .GroupBy(r => r.Score)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 1; i <= 5; i++)
            {
                if (!distribution.ContainsKey(i))
                    distribution[i] = 0;
            }

            return new DishRatingSummaryDto
            {
                DishId = dishId,
                AverageRating = Math.Round(ratings.Average(r => r.Score), 2),
                TotalRatings = ratings.Count,
                RatingDistribution = distribution
            };
        }

        private async Task UpdateDishAverageRatingAsync(int dishId)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                return;

            var avgRating = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .AverageAsync(r => (double?)r.Score);

            dish.Rating = avgRating.HasValue ? Math.Round(avgRating.Value, 2) : null;
            dish.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}