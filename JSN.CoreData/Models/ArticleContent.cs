using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JSN.CoreData.Models;

[Table("ArticleContent")]
[Index("ArticleId", Name = "UQ__ArticleC__9C6270E96B1E4B28", IsUnique = true)]
public class ArticleContent
{
    [Key] public Guid Id { get; set; }

    public int ArticleId { get; set; }

    public string? Content { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("ArticleContent")]
    public virtual Article Article { get; set; } = null!;
}