using System.ComponentModel.DataAnnotations;

namespace Dishapi.DAL.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DishId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CartId { get; set; }

        
        public Cart Cart { get; set; } = null!;
        public Dish Dish { get; set; } = null!;
    }
}