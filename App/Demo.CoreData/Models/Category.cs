using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("Category")]
public partial class Category
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Category Name
    /// </summary>
    [StringLength(500)]
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// Parent Category
    /// </summary>
    public int ParentId { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Article> Articles { get; } = new List<Article>();
}
