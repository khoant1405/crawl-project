using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Models;

[Table("User")]
[Index("UserName", Name = "UQ__User__C9F2845693F7C549", IsUnique = true)]
public class User
{
    [Key] public Guid Id { get; set; }

    [StringLength(50)] public string UserName { get; set; } = null!;

    [Required] public bool? IsActive { get; set; }

    [StringLength(50)] public string Role { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? TokenCreated { get; set; }

    public DateTime? TokenExpires { get; set; }
}