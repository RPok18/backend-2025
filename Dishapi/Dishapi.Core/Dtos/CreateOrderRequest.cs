namespace Dishapi.Core.Dtos
{
    public class CreateOrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
        public string? Notes { get; set; }
    }

    public class CreateOrderItem
    {
        public int DishId { get; set; }
        public int Quantity { get; set; }
    }
}