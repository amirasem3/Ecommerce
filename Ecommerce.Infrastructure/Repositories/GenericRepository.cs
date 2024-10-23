using System.Linq.Expressions;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class GenericRepository<TEntity> where TEntity:class
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
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy!=null)
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
        IQueryable<TEntity> query = _dbSet;
        foreach (var includeProperty in includeProperties.Split
                     (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        
        if (typeof(TEntity) == typeof(Category))
        {
            var categoryQuery = query as IQueryable<Category>;

            if (categoryQuery != null)
            {
                // Include subcategories and their subcategories
                return (await categoryQuery
                    .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.SubCategories)
                    .FirstOrDefaultAsync(entity => EF.Property<object>(entity, uniqueProperty).Equals(uniquePropertyValue)) as TEntity)!;
            }
            
        }

        if (typeof(TEntity) == typeof(Invoice))
        {
            var invoiceQuery = query as IQueryable<Invoice>;
            if (invoiceQuery != null)
            {
                return (await invoiceQuery
                    .Include(p => p.Products)
                    .ThenInclude(pi => pi.Product)
                    .FirstOrDefaultAsync(invoice =>
                        EF.Property<object>(invoice, uniqueProperty).Equals(uniquePropertyValue)) as TEntity)!;
            }
        }

        return (await query.FirstOrDefaultAsync(e => EF.Property<object>(e, uniqueProperty).Equals(uniquePropertyValue)))!;
    }

    public virtual async Task<TEntity> GetByIdAsync(object id, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;
       
        var splitIncludeProperties =
            includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var includeProperty in  splitIncludeProperties)
        {
            Console.WriteLine(includeProperty);
            query = query.Include(includeProperty);
        }

        if (typeof(TEntity) == typeof(Category))
        {
            var categoryQuery = query as IQueryable<Category>;

            if (categoryQuery != null)
            {
                // Include subcategories and their subcategories
                return (await categoryQuery
                    .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.SubCategories)
                    .FirstOrDefaultAsync(entity => EF.Property<object>(entity, "Id").Equals(id)) as TEntity)!;
            }
            
        }
        if (typeof(TEntity) == typeof(Invoice))
        {
            var invoiceQuery = query as IQueryable<Invoice>;
            if (invoiceQuery != null)
            {
                return (await invoiceQuery
                    .Include(p => p.Products)
                    .ThenInclude(pi => pi.Product)
                    .FirstOrDefaultAsync(invoice =>
                        EF.Property<object>(invoice, "Id").Equals(id)) as TEntity)!;
            }
        }
        return (await query
            .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id)))!;
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
       await _dbSet.AddAsync(entity);
    }

    public virtual async Task DeleteByIdAsync(object id)
    {
        TEntity targetEntity =  (await _dbSet.FindAsync(id))!;
       await Delete(targetEntity);
    }

    public virtual Task Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }

        _dbSet.Remove(entityToDelete);
        return Task.CompletedTask;
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}