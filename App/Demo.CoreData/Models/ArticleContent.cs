using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("ArticleContent")]
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
    [InverseProperty("ArticleContents")]
    public virtual Article Article { get; set; } = null!;
}
