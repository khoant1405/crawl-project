namespace JSN.Crawler.Common;

public class PaginatedList<T>
{
    public PaginatedList(List<T>? items, int count, int pageIndex, int pageSize)
    {
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Data = items!;
        PageIndex = pageIndex;
    }

    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; }
}