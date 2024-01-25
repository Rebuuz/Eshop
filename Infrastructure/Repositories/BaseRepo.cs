

using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepo<TEntity> where TEntity : class
{
    private readonly UserContext _userContext;

    protected BaseRepo(UserContext userContext)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// Add a new item
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            _userContext.Set<TEntity>().Add(entity);
            _userContext.SaveChanges();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Get all from the list
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            return _userContext.Set<TEntity>().ToList();
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Get one from the list
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return _userContext.Set<TEntity>().FirstOrDefault(predicate, null!); 
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    ///<summary>
    /// Update item in list
    ///  </summary>
    /// <param name = "entity" ></ param >
    /// < returns ></ returns >
    public virtual TEntity Update(TEntity entity)
    {
        try
        {
            var entityToUpdate = _userContext.Set<TEntity>().Find(entity);
            if (entityToUpdate != null)
            {
                entityToUpdate = entity;
                _userContext.Set<TEntity>().Update(entityToUpdate);
                _userContext.SaveChanges();

                return entityToUpdate;
            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /////Update 
    //public abstract TEntity Update(TEntity entity);


    /// <summary>
    /// Delete item in list
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _userContext.Set<TEntity>().FirstOrDefault(predicate);
            if (entity != null)
            {
                _userContext.Set<TEntity>().Remove(entity);
                _userContext.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return _userContext.Set<TEntity>().Any(predicate);
            
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }
}
