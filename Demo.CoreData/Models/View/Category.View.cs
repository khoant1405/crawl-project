namespace Demo.CoreData.Models.View;

public partial class CategoryView
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public int ParentId { get; set; }

    public string? UrlName { get; set; }
}
