using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;
using System;

namespace Dishapi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
            => _orderService = orderService;

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutRequest([FromBody] CheckoutRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var (success, error, order) = await _orderService.CheckoutAsync(userId, dto);
            if (!success) return BadRequest(new { error });

            var response = new OrderDto
            {
                Id = order!.Id,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                DeliveryAddress = order.DeliveryAddress,
                Status = order.Status,
                TotalAmount = (double)order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Notes = order.Notes,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    DishId = i.DishId,
                    DishName = i.DishName,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    Subtotal = (double)(i.Price * i.Quantity)
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!int.TryParse(userId, out var userIdInt)) return Unauthorized();
            if (order.UserId != userIdInt) return Forbid();

            var response = new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                DeliveryAddress = order.DeliveryAddress,
                Status = order.Status,
                TotalAmount = (double)order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Notes = order.Notes,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    DishId = i.DishId,
                    DishName = i.DishName,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    Subtotal = (double)(i.Price * i.Quantity)
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var orders = await _orderService.GetByUserAsync(userId);
            var result = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                CustomerPhone = o.CustomerPhone,
                CustomerEmail = o.CustomerEmail,
                DeliveryAddress = o.DeliveryAddress,
                Status = o.Status,
                TotalAmount = (double)o.TotalAmount,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                Notes = o.Notes,
                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    DishId = i.DishId,
                    DishName = i.DishName,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    Subtotal = (double)(i.Price * i.Quantity)
                }).ToList()
            }).ToList();

            return Ok(result);
        }
    }
}