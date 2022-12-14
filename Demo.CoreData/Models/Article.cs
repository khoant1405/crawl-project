using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("Article")]
[Index("ArticleName", Name = "Nidx_Article_ArticleName")]
public partial class Article
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(500)]
    public string ArticleName { get; set; } = null!;

    [StringLength(255)]
    public string Status { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public Guid? CreationBy { get; set; }

    [Column("RefURL")]
    [StringLength(255)]
    public string RefUrl { get; set; } = null!;

    [StringLength(255)]
    public string? ImageThumb { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int IdDisplay { get; set; }

    [InverseProperty("Article")]
    public virtual ArticleContent? ArticleContent { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Articles")]
    public virtual Category Category { get; set; } = null!;
}
