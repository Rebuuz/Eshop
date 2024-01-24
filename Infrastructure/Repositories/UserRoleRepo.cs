

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRoleRepo(UserContext userContext) : BaseRepo<UserRoleEntity>(userContext)
{

    private readonly UserContext _userContext = userContext;

    public override IEnumerable<UserRoleEntity> GetAll()
    {
        try
        {
            return _userContext.UserRoles.Include(x => x.UserId);
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override UserRoleEntity GetOne(Expression<Func<UserRoleEntity, bool>> predicate)
    {
        return base.GetOne(predicate);
    }
}
