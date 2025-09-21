namespace Dishapi.Models
{
    public class DishResponse
    {
        public List<Dish> Dishes { get; set; } = new();
        public Pagination Pagination { get; set; } = new();
    }
}
