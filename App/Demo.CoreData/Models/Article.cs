using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("Article")]
[Index("ArticleName", Name = "Nidx_Article_ArticleName")]
public partial class Article
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Article Name
    /// </summary>
    [StringLength(500)]
    public string ArticleName { get; set; } = null!;

    /// <summary>
    /// Article Status : EDIT, PUBLISH, DELETE...
    /// </summary>
    [StringLength(255)]
    public string Status { get; set; } = null!;

    /// <summary>
    /// The datetime when article record is inserted
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// User create article
    /// </summary>
    public Guid? CreationBy { get; set; }

    /// <summary>
    /// The datetime when article is save
    /// </summary>
    public DateTime? LastSaveDate { get; set; }

    /// <summary>
    /// RefURL
    /// </summary>
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
