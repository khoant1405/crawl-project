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

        public PaginatedList(List<T>? items, int count, int pageIndex, int pageSize, int numberOfPagesShow)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPreviousPage = pageIndex > 1;
            HasNextPage = pageIndex < TotalPages;
            Data = items!;
            PageIndex = pageIndex;
            if (numberOfPagesShow >= TotalPages)
            {
                ShowFromPage = 1;
                ShowToPage = TotalPages;
            } else
            {
                ShowFromPage = pageIndex - 2 > 0 ? pageIndex - 2 : 1;
                ShowToPage = pageIndex + 2 > TotalPages ? TotalPages : pageIndex + 2;
                while (ShowToPage - ShowFromPage + 1 < numberOfPagesShow)
                {
                    if (ShowToPage < TotalPages)
                    {
                        ShowToPage++;
                    }
                    else if (ShowFromPage > 1)
                    {
                        ShowFromPage--;
                    }
                }
            }
        }
    }
}
