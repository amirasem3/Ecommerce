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
    
    public async Task<Category> GetCategoryById(Guid id)
    {
        return (await _context.Categories
            .Include(cat => cat.SubCategories)
            .FirstOrDefaultAsync(cat => cat.Id == id))!;
    }

    public async Task AddSubCategoryAsync(Category parent, Category child)
    {
        parent.SubCategories.Add(child);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> GetParentByChildId(Guid id)
    {
        var catChild = await GetCategoryById(id);
        return (await _context.Categories
            .Include(cat => cat.SubCategories)
            .FirstOrDefaultAsync(cat => cat.Type == true && cat.Id == catChild.ParentCategoryId))!;
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

    

    public async Task<Category> GetCategoryByName(string name)
    {
       return  (await _context.Categories
           .Include(cat => cat.SubCategories)
           .FirstOrDefaultAsync(cat => cat.Name == name))!;
       
    }

    public async Task<IEnumerable<Category>> GetSubCategory(Guid parentId)
    {
       return await _context.Categories
           .Include(e =>e.SubCategories)
            .Where(e => e.ParentCategoryId == parentId).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _context.Categories
            .Include(cat => cat.SubCategories)
            .ToListAsync();
    }

    public async Task<bool> DeleteCategoryById(Guid id)
    {
        var category = await GetCategoryById(id);
        
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;    

    }
    
}