namespace Dishapi.Core.Dtos
{
    public class CheckoutRequestDto
    {
        
        public DateTimeOffset DeliveryAt { get; set; }

       
        public string DeliveryAddress { get; set; } = string.Empty;

       
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

       
        public string? Notes { get; set; }

      
        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
    }
}
