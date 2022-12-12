namespace Demo.CoreData.Models.View;

public partial class ArticlePagination
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int ShowFromPage { get; set; }
    public int ShowToPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public List<ArticleView> Data { get; set; }
}
