﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JSN.CoreData.Models;

[Table("ArticleContent")]
[Index("ArticleId", Name = "UQ__ArticleC__9C6270E9AEEB61EB", IsUnique = true)]
public class ArticleContent
{
    [Key] public Guid Id { get; set; }

    public Guid ArticleId { get; set; }

    public string? Content { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("ArticleContent")]
    public virtual Article Article { get; set; } = null!;
}