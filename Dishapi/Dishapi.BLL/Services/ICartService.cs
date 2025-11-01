using System.Threading.Tasks;
using Dishapi.Core.Dtos;

namespace Dishapi.BLL.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int cartId);
        Task<CartDto> AddDishAsync(int cartId, int dishId, bool increase = true);
        Task<CartDto> RemoveDishAsync(int cartId, int dishId, bool increase = false);
    }
}
