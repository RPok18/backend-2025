namespace Dishapi.Models
{
    public class Pagination
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public Pagination()
        {
        }

        public Pagination(int currentPage, int pageSize, int totalItems)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            HasNext = CurrentPage < TotalPages;
            HasPrevious = CurrentPage > 1;
        }
    }
}