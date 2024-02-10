

namespace Infrastructure.Dtos;

public class AuthenticationDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
