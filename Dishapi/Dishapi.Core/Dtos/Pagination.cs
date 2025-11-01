namespace Dishapi.Core.Dtos
{
    public class Pagination
    {
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages => PageSize > 0 ? (int)System.Math.Ceiling((double)TotalItems / PageSize) : 0;

        public Pagination(int currentPage, int pageSize, int totalItems)
        {
            CurrentPage = System.Math.Max(1, currentPage);
            PageSize = System.Math.Max(1, pageSize);
            TotalItems = System.Math.Max(0, totalItems);
        }
    }
}