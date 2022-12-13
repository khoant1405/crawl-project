namespace Demo.Crawler.Common
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int ShowFromPage { get; set; }
        public int ShowToPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Data { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPreviousPage = pageIndex > 1;
            HasNextPage = pageIndex < TotalPages;
            Data = items;
            PageIndex = pageIndex;
            ShowFromPage = 0;
            ShowToPage = 0;
        }
    }
}
