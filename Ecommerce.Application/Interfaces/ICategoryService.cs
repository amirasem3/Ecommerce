using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface ICategoryService
{
    public Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto updateCategoryDto);
    public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    public Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(Guid parentCategory);
    public Task<CategoryDto> GetCategoryByNameAsync(string name);

    public Task<CategoryDto> GetParentCategoryAsync(Guid childId);

    public Task<CategoryDto> GetCategoryByIdAsync(Guid id);

    public Task<CategoryDto> UpdateCategoryAsync(Guid id, AddUpdateCategoryDto updateCategoryDto);
    public Task<bool> DeleteCategoryByIdAsync(Guid id);

}