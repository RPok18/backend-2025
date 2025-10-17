namespace Dishapi.Core.Dtos
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool Vegetarian { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}