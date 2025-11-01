namespace Dishapi.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }
}