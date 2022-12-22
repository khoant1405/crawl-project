namespace Demo.CoreData.ViewModels;

public class CategoryView
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public int ParentId { get; set; }

    public string? UrlName { get; set; }
}