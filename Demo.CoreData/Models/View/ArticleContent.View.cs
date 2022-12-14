namespace Demo.CoreData.Models.View;

public partial class ArticleContentView
{
    public Guid Id { get; set; }

    public Guid ArticleId { get; set; }

    public string? Content { get; set; }
}
