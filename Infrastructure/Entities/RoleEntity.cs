using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class RoleEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserEntity> User { get; set; }  = new List<UserEntity>();
}
