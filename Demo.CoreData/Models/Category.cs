using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string CategoryName { get; set; } = null!;

    public int? ParentId { get; set; }

    [StringLength(500)]
    public string UrlName { get; set; } = null!;
}
