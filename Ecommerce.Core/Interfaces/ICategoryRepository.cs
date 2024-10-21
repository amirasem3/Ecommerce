using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface ICategoryRepository
{
    public Task<Category> AddNewCategory(Category category);

    public Task<Category> UpdateCategory(Category category);
    public Task<Category> GetCategoryByName(string name);

    public Task<Category> GetCategoryById(Guid id);
    
    public Task AddSubCategoryAsync(Category parent, Category child);
    public Task<Category> GetParentByChildId(Guid id);

    public Task<IEnumerable<Category>> GetAllCategories();

    public Task<bool> DeleteCategoryById(Guid id);
    

}