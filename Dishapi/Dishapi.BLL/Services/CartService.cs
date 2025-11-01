using System.Linq;
using System.Threading.Tasks;
using Dishapi.DAL;
using Dishapi.DAL.Entities;
using Dishapi.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dishapi.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Dish)
                .ToListAsync();

            var items = cartItems.Select(c => new CartItemDto
            {
                Id = c.Id,
                DishId = c.DishId,
                Name = c.Dish.Name,
                ImageUrl = c.Dish.ImageUrl ?? string.Empty,
                Price = (double)c.Price,
                Quantity = c.Quantity,
                Subtotal = (double)(c.Price * c.Quantity)
            }).ToList();

            return new CartDto
            {
                Items = items,
                TotalAmount = cartItems.Sum(c => c.Price * c.Quantity),
                TotalItems = cartItems.Sum(c => c.Quantity)
            };
        }

        public async Task<CartDto> AddDishAsync(int userId, int dishId, bool increase = true)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
            {
                throw new Exception("Dish not found");
            }

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);

            if (item == null)
            {
               
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId.ToString());

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId.ToString(),
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                item = new CartItem
                {
                    UserId = userId,
                    DishId = dishId,
                    Quantity = 1,
                    Price = dish.Price,
                    CartId = cart.Id
                };
                _context.CartItems.Add(item);
            }
            else if (increase)
            {
                item.Quantity += 1;
            }

            await _context.SaveChangesAsync();
            return await GetCartAsync(userId);
        }

        public async Task<CartDto> RemoveDishAsync(int userId, int dishId, bool decrease = true)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);

            if (item == null)
                return await GetCartAsync(userId);

            if (decrease && item.Quantity > 1)
                item.Quantity -= 1;
            else
                _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();
            return await GetCartAsync(userId);
        }
    }
}