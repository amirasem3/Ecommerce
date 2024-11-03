using System.Linq.Expressions;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Core.Log;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace Ecommerce.Infrastructure.Repositories;

public class GenericRepository<TEntity> where TEntity : class
{
    internal EcommerceDbContext _context;
    internal DbSet<TEntity> _dbSet;

    public GenericRepository(EcommerceDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
    {
        
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to get multiple of {entityType} with filter {filter}",args:[orderBy,includeProperties]);
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public async Task<TEntity> GetByUniquePropertyAsync(string uniqueProperty = "",
        string includeProperties = "", string uniquePropertyValue = "")
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to get {entityType} by its UniqueProperty.", args:
            [uniqueProperty, uniquePropertyValue, includeProperties]);
        IQueryable<TEntity> query = _dbSet;
        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }


        return (await query.FirstOrDefaultAsync(e =>
            EF.Property<object>(e, uniqueProperty).Equals(uniquePropertyValue)))!;
    }

    public virtual async Task<TEntity> GetByIdAsync(object id, string includeProperties = "")
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to get entity with type {entityType} by its ID.",
            args: [id, includeProperties]);
        IQueryable<TEntity> query = _dbSet;

        var splitIncludeProperties =
            includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var includeProperty in splitIncludeProperties)
        {
            Console.WriteLine(includeProperty);
            query = query.Include(includeProperty);
        }

        return (await query
            .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id)))!;
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to add an new entity with type {entityType}", args: [entity]);
        await _dbSet.AddAsync(entity);
        LoggerHelper.LogWithDetails("Successful Insert.",args:[entity]);
    }

    public virtual async Task DeleteByIdAsync(object id)
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to Delete a {entityType}", args: [id]);
        TEntity targetEntity = (await _dbSet.FindAsync(id))!;
        await Delete(targetEntity);
    }

    public virtual Task Delete(TEntity entityToDelete)
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to Delete a {entityType}", args: [entityToDelete]);
        
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }

        _dbSet.Remove(entityToDelete);
        LoggerHelper.LogWithDetails("Successful Delete.",args:[entityToDelete]);
        return Task.CompletedTask;
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        var entityType = typeof(TEntity);
        LoggerHelper.LogWithDetails($"Attempt to Update a {entityType}", args: [entityToUpdate]);
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
        LoggerHelper.LogWithDetails($"Successful Update.",args:[entityToUpdate]);
    }
}