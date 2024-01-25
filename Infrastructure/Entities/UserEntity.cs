
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

/// <summary>
/// Entity for my User table
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
    public int AddressId { get; set; }

    public virtual AddressEntity Address { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;


    public virtual AuthenticationEntity Authentication { get; set; } = null!;

    public virtual ContactInformationEntity ContactInformation { get; set; } = null!;
}
