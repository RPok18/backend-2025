using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart([FromHeader(Name = "X-Cart-Id")] string? cartId)
        {
            if (string.IsNullOrEmpty(cartId) || !int.TryParse(cartId, out int cartIdInt))
            {
                return BadRequest(new { message = "Invalid or missing Cart ID" });
            }

            var result = await _cartService.GetCartAsync(cartIdInt);
            return Ok(result);
        }

        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddDishToBasket(
            int dishId,
            [FromQuery] bool increase = true,
            [FromHeader(Name = "X-Cart-Id")] string? cartId = null)
        {
            int cartIdInt = 0;
            if (!string.IsNullOrEmpty(cartId) && int.TryParse(cartId, out int parsedCartId))
            {
                cartIdInt = parsedCartId;
            }

            var result = await _cartService.AddDishAsync(cartIdInt, dishId, increase);

            if (string.IsNullOrEmpty(cartId))
                Response.Headers.Add("X-Cart-Id", result.CartId.ToString());

            return Ok(result);
        }

        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> RemoveDishFromBasket(
            int dishId,
            [FromQuery] bool increase = false,
            [FromHeader(Name = "X-Cart-Id")] string? cartId = null)
        {
            if (string.IsNullOrEmpty(cartId) || !int.TryParse(cartId, out int cartIdInt))
            {
                return BadRequest(new { message = "Invalid or missing Cart ID" });
            }

            var result = await _cartService.RemoveDishAsync(cartIdInt, dishId, increase);
            return Ok(result);
        }
    }
}