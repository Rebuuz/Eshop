

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRepo(UserContext userContext) : BaseRepo<UserEntity>(userContext)
{

    private readonly UserContext _userContext = userContext;

    public override IEnumerable<UserEntity> GetAll()
    {
        try
        {
            return _userContext.Users.Include(x => x.)
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        return base.GetOne(predicate);
    }
}
