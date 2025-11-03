using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return Ok(new CartDto
                {
                    CartId = userId,
                    Items = new List<CartItemDto>(),
                    TotalAmount = 0,
                    TotalItems = 0
                });
            }

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            try
            {
                var result = await _cartService.AddToCartAsync(userId, request.DishId, request.Quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("items/{dishId}")]
        public async Task<IActionResult> UpdateCartItem(int dishId, [FromBody] UpdateCartItemRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound(new { message = "Cart not found" });

            var cartItem = cart.Items.FirstOrDefault(i => i.DishId == dishId);
            if (cartItem == null)
                return NotFound(new { message = "Dish not found in cart" });

            var result = await _cartService.UpdateCartItemAsync(userId, cartItem.Id, request.Quantity);

            if (!result)
                return NotFound(new { message = "Failed to update cart item" });

            return Ok(new { message = "Cart item updated successfully" });
        }

        [HttpDelete("items/{dishId}")]
        public async Task<IActionResult> RemoveFromCart(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound(new { message = "Cart not found" });

            var cartItem = cart.Items.FirstOrDefault(i => i.DishId == dishId);
            if (cartItem == null)
                return NotFound(new { message = "Dish not found in cart" });

            var result = await _cartService.RemoveFromCartAsync(userId, cartItem.Id);

            if (!result)
                return NotFound(new { message = "Failed to remove cart item" });

            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            await _cartService.ClearCartAsync(userId);
            return Ok(new { message = "Cart cleared successfully" });
        }
    }
}