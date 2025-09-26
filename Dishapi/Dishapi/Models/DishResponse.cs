namespace Dishapi.Models
{
    public class DishResponse
    {
        public List<DishDto> Dishes { get; set; } = new();
        public Pagination Pagination { get; set; } = new();
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public DishResponse()
        {
        }

        public DishResponse(List<DishDto> dishes, Pagination pagination)
        {
            Dishes = dishes;
            Pagination = pagination;
            Success = true;
        }

        public DishResponse(string message, bool success = false)
        {
            Message = message;
            Success = success;
        }
    }
}