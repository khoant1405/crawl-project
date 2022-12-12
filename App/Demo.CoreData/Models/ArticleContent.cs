using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("ArticleContent")]
[Index("ArticleId", Name = "UQ__ArticleC__9C6270E936846DA0", IsUnique = true)]
public partial class ArticleContent
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// ArticleId
    /// </summary>
    public Guid ArticleId { get; set; }

    /// <summary>
    /// Article Content
    /// </summary>
    public string? Content { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("ArticleContent")]
    public virtual Article Article { get; set; } = null!;
}
