namespace JSN.CoreData.ViewModels;

public class ArticleView
{
    public int Id { get; set; }

    public string ArticleName { get; set; } = null!;

    //public string Status { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    //public Guid? CreationBy { get; set; }

    public string RefUrl { get; set; } = null!;

    public string? ImageThumb { get; set; }

    public string? Description { get; set; }

    //public int CategoryId { get; set; }
}