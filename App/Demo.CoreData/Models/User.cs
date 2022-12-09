using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("User")]
public partial class User
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// First Name
    /// </summary>
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last Name
    /// </summary>
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    [StringLength(255)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// IsActive
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }

    [StringLength(100)]
    public string Password { get; set; } = null!;
}
