using System;

namespace Dishapi.DAL.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CartId { get; set; }

        public Dish Dish { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
    }
}