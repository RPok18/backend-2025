namespace Dishapi.Core.Dtos
{
    public class DishCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? NameEn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? CategoryEn { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool Vegetarian { get; set; }
        public double Rating { get; set; }
    }
}