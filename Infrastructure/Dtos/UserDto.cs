

namespace Infrastructure.Dtos;

public class UserDto
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string StreetName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string RoleName { get; set; } = null!;

    public Guid Id { get; set; }

    /// <summary>
    /// Authentication 
    /// </summary>
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public RoleDto RoleDto { get; set; } = null!;
    public AddressDto AddressDto { get; set; } = null!;

    public AuthenticationDto AuthenticationDto { get; set; } = null!;
    public ContactInformationDto ContactInformationDto { get; set; } = null!;
}                                                                                                               
