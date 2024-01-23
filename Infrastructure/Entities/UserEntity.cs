
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

/// <summary>
/// Entities for my database (tables)
/// </summary>
/// 

[Index(nameof(Email), IsUnique = true )]
public class UserEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(AddressEntity))]
    public int AddressId { get; set; }

    public virtual AddressEntity Address { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!; 
}
