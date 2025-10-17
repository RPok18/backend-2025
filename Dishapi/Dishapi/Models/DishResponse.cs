using Dishapi.Core.Dtos;

namespace Dishapi.Models
{
    public class DishResponse
    {
        public List<DishDto>? Dishes { get; set; }
        public Pagination? Pagination { get; set; }
        public string? Error { get; set; }
        public bool Success => string.IsNullOrEmpty(Error);

        public DishResponse() { }

        public DishResponse(List<DishDto> dishes, Pagination pagination)
        {
            Dishes = dishes;
            Pagination = pagination;
        }

        public DishResponse(string error)
        {
            Error = error;
        }
    }

    
}
