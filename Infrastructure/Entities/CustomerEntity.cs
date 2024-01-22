
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CustomerEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    
}
