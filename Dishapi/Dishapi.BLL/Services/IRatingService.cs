using Dishapi.Core.Dtos;

namespace Dishapi.BLL.Services
{
    public interface IRatingService
    {
        Task<RatingResponseDto> CreateRatingAsync(string userId, int dishId, RatingCreateDto ratingDto);
        Task<RatingResponseDto?> GetUserRatingForDishAsync(string userId, int dishId);
        Task<List<RatingDto>> GetRatingsForDishAsync(int dishId, int page = 1, int pageSize = 10);
        Task<DishRatingSummaryDto> GetDishRatingSummaryAsync(int dishId);
        Task<RatingResponseDto> UpdateRatingAsync(string userId, int ratingId, RatingUpdateDto ratingDto);
        Task<bool> DeleteRatingAsync(string userId, int ratingId);
    }
}