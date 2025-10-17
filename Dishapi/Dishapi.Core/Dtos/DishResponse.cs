using System;
using System.Collections.Generic;
using Dishapi.Core.Dtos;

namespace Dishapi.Models
{
    public class DishResponse
    {
        public List<DishDto>? Dishes { get; set; }
        public WebPagination? Pagination { get; set; }
        public string? Error { get; set; }
        public bool Success => string.IsNullOrEmpty(Error);

        public DishResponse() { }

        public DishResponse(List<DishDto> dishes, WebPagination pagination)
        {
            Dishes = dishes;
            Pagination = pagination;
        }

        public DishResponse(string error)
        {
            Error = error;
        }
    }

    
    public class WebPagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public WebPagination(int page, int pageSize, int totalItems)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        
        public WebPagination() { }
    }
}
