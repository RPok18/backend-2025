
using System;
using System.Collections.Generic;

namespace Dishapi.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public List<OrderItemDto> Items { get; set; } = new(); // ← Add initializer
        public string Status { get; set; } = string.Empty; // ← Add initializer
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? DeliveryAddress { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Notes { get; set; }
    }
}