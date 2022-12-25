using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSN.CoreData.Models;

[Table("Article")]
public class Article
{
    [Key] public int Id { get; set; }

    [StringLength(500)] public string ArticleName { get; set; } = null!;

    [StringLength(255)] public string Status { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Guid CreationBy { get; set; }

    [Column("RefURL")] [StringLength(255)] public string RefUrl { get; set; } = null!;

    [StringLength(255)] public string? ImageThumb { get; set; }

    [StringLength(500)] public string? Description { get; set; }

    public int CategoryId { get; set; }

    [InverseProperty("Article")] public virtual ArticleContent? ArticleContent { get; set; }
}