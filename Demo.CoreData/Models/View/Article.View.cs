namespace Demo.CoreData.Models.View;

public partial class ArticleView
{
    public Guid Id { get; set; }

    public string ArticleName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public Guid? CreationBy { get; set; }

    public string RefUrl { get; set; } = null!;

    public string? ImageThumb { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int IdDisplay { get; set; }
}
