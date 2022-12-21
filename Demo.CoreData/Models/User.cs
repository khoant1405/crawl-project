using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("User")]
public partial class User
{
    [Key]
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    [Required]
    public bool? IsActive { get; set; }

    public int? Role { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? TokenCreated { get; set; }

    public DateTime? TokenExpires { get; set; }
}
