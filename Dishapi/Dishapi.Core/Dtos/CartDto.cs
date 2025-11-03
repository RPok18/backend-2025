namespace Dishapi.Core.Dtos
{
    public class CartDto
    {
        public string CartId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string DishName { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }
    }

    public class AddToCartRequest
    {
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}