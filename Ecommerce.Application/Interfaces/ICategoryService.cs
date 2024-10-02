using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface ICategoryService
{
    public Task<CategoryDto> AddCategoryAsync(AddCategoryDto categoryDto);
    public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    public Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(Guid parentCategory);
    public Task<CategoryDto> GetCategoryByNameAsync(string name);

    public Task<CategoryDto> GetParentCategoryAsync(Guid childId);

    public Task<CategoryDto> GetCategoryByIdAsync(Guid id);
    public Task<bool> DeleteCategoryById(Guid id);

}