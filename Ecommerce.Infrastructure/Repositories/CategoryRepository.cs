using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _context;

    public CategoryRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task<Category> AddNewCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task AddSubCatToParent(Guid parentId, Guid childId)
    {
        var child = await _context.Categories.FindAsync(childId);
        _context.Categories
            .Include(e => e.SubCategories)
            .Where(e => e.ParentCategoryId == parentId)
            .Append(new Category
            {
                Id = child.Id,
                Name = child.Name,
                ParentCategoryId = parentId,
                SubCategories = child.SubCategories,
                Type = child.Type,
            });
        await _context.SaveChangesAsync();

    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> GetParentByChildId(Guid id)
    {
        var catChild = await _context.Categories.FindAsync(id);
        return await _context.Categories.FirstOrDefaultAsync(cat => cat.Id == catChild.ParentCategoryId);
    }

    public async Task<Category> GetCategoryByName(string name)
    {
       return  (await _context.Categories.FirstOrDefaultAsync(cat => cat.Name == name))!;
       
    }

    public async Task<IEnumerable<Category>> GetSubCategory(Guid parentId)
    {
       return await _context.Categories.Include(e =>e.SubCategories)
            .Where(e => e.ParentCategoryId == parentId).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<bool> DeleteCategoryById(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category!=null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;    
        }

        return false;

    }
}