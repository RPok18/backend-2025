using Dishapi.Core.Dtos;

namespace Dishapi.BLL.Services
{
    public interface IDishService
    {
        Task<IEnumerable<DishDto>> GetAllDishesAsync();
        Task<DishDto?> GetDishByIdAsync(int id);
        Task<DishDto> CreateDishAsync(DishCreateDto createDishDto);
        Task<DishDto?> UpdateDishAsync(int id, DishUpdateDto updateDishDto);
        Task<bool> DeleteDishAsync(int id);
        Task<IEnumerable<DishDto>> GetDishesByProfileIdAsync(int profileId);
    }
}