using Dishapi.Core.Dtos; 

namespace Dishapi.Core.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new(); 
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}