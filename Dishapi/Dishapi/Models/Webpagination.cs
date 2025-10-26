namespace Dishapi.Models
{
    public class WebPagination
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize > 0 ? (int)System.Math.Ceiling((double)TotalItems / PageSize) : 0;
    }
}
