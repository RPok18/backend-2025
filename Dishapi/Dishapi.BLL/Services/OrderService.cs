using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dishapi.Core.Dtos;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Dishapi.BLL.Configuration;

namespace Dishapi.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        private readonly DeliveryOptions _deliveryOptions;

        public OrderService(AppDbContext db, IOptions<DeliveryOptions> deliveryOptions)
        {
            _db = db;
            _deliveryOptions = deliveryOptions.Value;
        }

        // Match the interface: accepts CheckoutRequestDto
        public async Task<(bool Success, string? Error, Order? Order)> CheckoutAsync(string userId, CheckoutRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return (false, "User is not authenticated.", null);

            if (!int.TryParse(userId, out int userIdInt))
                return (false, "Invalid user ID.", null);

            // Load the user's cart (with Dish on cart items, if any)
            var cart = await _db.Carts
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.Dish)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
                return (false, "Cart is empty.", null);

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userIdInt,
                    CustomerName = dto.CustomerName,
                    CustomerPhone = dto.CustomerPhone,
                    CustomerEmail = dto.CustomerEmail,
                    DeliveryAddress = dto.DeliveryAddress ?? string.Empty,
                    Notes = dto.Notes ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Placed",
                    OrderItems = new List<OrderItem>()
                };

                decimal total = 0m;

                foreach (var ci in cart.Items)
                {
                    var oi = new OrderItem
                    {
                        DishId = ci.DishId,
                        DishName = ci.Dish?.Name ?? string.Empty,
                        Price = ci.Price,
                        Quantity = ci.Quantity
                    };

                    order.OrderItems.Add(oi);
                    total += ci.Price * ci.Quantity;
                }

                order.TotalAmount = total;

                _db.Orders.Add(order);

                // clear cart items
                _db.CartItems.RemoveRange(cart.Items);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return (true, null, order);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return (false, "Failed to place order: " + ex.Message, null);
            }
        }

        // Match the interface: return entity collection
        public async Task<IEnumerable<Order>> GetByUserAsync(string userId)
        {
            if (!int.TryParse(userId, out int userIdInt))
                return Enumerable.Empty<Order>();

            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userIdInt)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders;
        }

        
        public async Task<Order?> GetByIdAsync(int orderId)
        {
            var o = await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return o;
        }
    }
}
