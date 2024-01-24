

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RoleRepo(UserContext userContext) : BaseRepo<RoleEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;

    public override IEnumerable<RoleEntity> GetAll()
    {
        try
        {
            return _userContext.Roles.Include(x => x.)
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override RoleEntity GetOne(Expression<Func<RoleEntity, bool>> predicate)
    {
        return base.GetOne(predicate);
    }
}

