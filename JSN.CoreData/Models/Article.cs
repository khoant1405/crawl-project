using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JSN.CoreData.Models;

[Table("Article")]
[Index("IdDisplay", Name = "UQ__Article__BEDCC22AAFA6C8A7", IsUnique = true)]
public class Article
{
    [Key] public Guid Id { get; set; }

    [StringLength(500)] public string ArticleName { get; set; } = null!;

    [StringLength(255)] public string Status { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Guid CreationBy { get; set; }

    [Column("RefURL")] [StringLength(255)] public string RefUrl { get; set; } = null!;

    [StringLength(255)] public string? ImageThumb { get; set; }

    [StringLength(500)] public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int IdDisplay { get; set; }

    [InverseProperty("Article")] public virtual ArticleContent? ArticleContent { get; set; }
}