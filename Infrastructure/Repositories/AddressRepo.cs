

using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class AddressRepo(UserContext userContext) : BaseRepo<AddressEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;
}
