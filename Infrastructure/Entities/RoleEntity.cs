﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class RoleEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string RoleName { get; set; } = null!;


    public virtual ICollection<UserEntity> Users { get; set; }  = new List<UserEntity>();
}
