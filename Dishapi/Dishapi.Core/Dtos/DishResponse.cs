using System.Collections.Generic;

namespace Dishapi.Core.Dtos
{
    public class DishResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Error { get; set; }
        public List<T>? Items { get; set; }
        public Pagination? Pagination { get; set; }

        public DishResponse()
        {
            Success = true;
        }

        public DishResponse(List<T> items, Pagination pagination)
        {
            Success = true;
            Items = items;
            Pagination = pagination;
        }

        public DishResponse(string errorMessage)
        {
            Success = false;
            Error = errorMessage;
            Items = new List<T>();
            Pagination = new Pagination(1, 10, 0);
        }
    }
}
