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

        public async Task<CartDto?> GetCartByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            if (!int.TryParse(userId, out var userIdInt))
                return null;

            return await GetCartAsync(userIdInt, userId);
        }

        public async Task<CartDto> AddToCartAsync(string userId, int dishId, int quantity)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be empty");

            if (!int.TryParse(userId, out var userIdInt))
                throw new ArgumentException("Invalid user ID format");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new Exception("Dish not found");

           
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userIdInt && c.DishId == dishId);

            if (existingItem != null)
            {
                
                existingItem.Quantity += quantity;
                existingItem.Price = dish.Price;
            }
            else
            {
               
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId, 
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                
                var newItem = new CartItem
                {
                    UserId = userIdInt, 
                    DishId = dishId,
                    Quantity = quantity,
                    Price = dish.Price,
                    CartId = cart.Id
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return await GetCartAsync(userIdInt, userId);
        }

        public async Task<bool> UpdateCartItemAsync(string userId, int itemId, int quantity)
        {
            if (!int.TryParse(userId, out var userIdInt))
                return false;

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == itemId && c.UserId == userIdInt);

            if (item == null)
                return false;

            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int itemId)
        {
            if (!int.TryParse(userId, out var userIdInt))
                return false;

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == itemId && c.UserId == userIdInt);

            if (item == null)
                return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ClearCartAsync(string userId)
        {
            if (!int.TryParse(userId, out var userIdInt))
                return;

            var items = await _context.CartItems
                .Where(c => c.UserId == userIdInt)
                .ToListAsync();

            if (items.Any())
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }

       
        private async Task<CartDto> GetCartAsync(int userIdInt, string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userIdInt)
                .Include(c => c.Dish)
                .ToListAsync();

            var items = cartItems.Select(c => new CartItemDto
            {
                Id = c.Id,
                DishId = c.DishId,
                DishName = c.Dish.Name,
                Price = (double)c.Price,
                Quantity = c.Quantity,
                Subtotal = (double)(c.Price * c.Quantity)
            }).ToList();

            return new CartDto
            {
                CartId = userId, 
                 
                Items = items,
                TotalAmount = (decimal)cartItems.Sum(c => c.Price * c.Quantity),
                TotalItems = cartItems.Sum(c => c.Quantity)
            };
        }
    }
}