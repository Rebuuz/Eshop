
using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class AuthenticationRepo(UserContext userContext) : BaseRepo<AuthenticationEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;
}
