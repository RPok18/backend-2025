using Dishapi.Core.Dtos;
using System.Threading.Tasks;

namespace Dishapi.BLL.Services
{
    public interface ICartService
    {
        Task<CartDto?> GetCartByUserIdAsync(string userId);
        Task<CartDto> AddToCartAsync(string userId, int dishId, int quantity);
        Task<bool> UpdateCartItemAsync(string userId, int itemId, int quantity);
        Task<bool> RemoveFromCartAsync(string userId, int itemId);
        Task ClearCartAsync(string userId);
    }
}