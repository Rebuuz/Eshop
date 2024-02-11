

using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;
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
    /// Create async method
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            _userContext.Set<TEntity>().Add(entity);
             await _userContext.SaveChangesAsync();
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
            var result = _userContext.Set<TEntity>().FirstOrDefault(predicate);
            return result!;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Get one async method
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual async Task <TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await _userContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return result!;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    ///<summary>
    /// Update item in list
    ///  </summary>
    /// <param name = "entity" ></ param >
    /// < returns ></ returns >
    public TEntity Update(Expression<Func<TEntity, bool>> expression, TEntity entity)
    {
        try
        {
            var entityToUpdate = _userContext.Set<TEntity>().FirstOrDefault(expression, entity);
            if (entityToUpdate != null)
            {
                entityToUpdate = entity;
                _userContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                _userContext.SaveChanges();

                return entityToUpdate;
            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Update async method
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity entity)
    {
        try
        {
            var entityToUpdate = await _userContext.Set<TEntity>().FirstOrDefaultAsync(expression);
            if (entityToUpdate != null)
            {
                _userContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                await _userContext.SaveChangesAsync();

                return entityToUpdate;
            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }


    /// <summary>
    /// Update one async method
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> UpdateOneAsync(TEntity entity)
    {
        try
        {
            _userContext.Set<TEntity>().Update(entity);
            await _userContext.SaveChangesAsync();
            return entity;
        }
        catch { }
        return null!;
    }





        /// <summary>
        /// Delete item in list
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
    public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var entity = _userContext.Set<TEntity>().FirstOrDefault(expression);
            _userContext.Remove(entity!);
            _userContext.SaveChanges();

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }

    /// <summary>
    /// If exists method
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return _userContext.Set<TEntity>().Any(predicate);
            
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }
}
