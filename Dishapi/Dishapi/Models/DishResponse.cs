using Dishapi.Core.Dtos;

namespace Dishapi.Models
{
    public class DishResponse
    {
        public List<DishDto> Items { get; set; }
        public Pagination Pagination { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

        public DishResponse(List<DishDto> items, Pagination pagination)
        {
            Items = items;
            Pagination = pagination;
            Success = true;
            Message = null;
        }

        public DishResponse(string errorMessage)
        {
            Items = new List<DishDto>();
            Pagination = new Pagination(1, 1, 0);
            Success = false;
            Message = errorMessage;
        }
    }
}