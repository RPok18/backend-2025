using Dishapi.Core.Dtos;
using Dishapi.DAL.Entities;

namespace Dishapi.BLL.Services
{
    public interface IOrderService
    {
        
        Task<(bool Success, string? Error, Order? Order)> CheckoutAsync(string userId, CheckoutRequestDto dto);

        
        Task<IEnumerable<Order>> GetByUserAsync(string userId);

       
        Task<Order?> GetByIdAsync(int orderId);
    }
}
