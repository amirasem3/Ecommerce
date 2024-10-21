using System.ComponentModel.DataAnnotations;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class CategoryServices : ICategoryService
{
    public const string CategoryException = "Category Not Found!";
    public const string ParentCategoryException = "Parent Category Not Found!";
    private readonly ICategoryRepository _categoryRepository;

    public CategoryServices(ICategoryRepository categoryRepository)
    {
        this._categoryRepository = categoryRepository;
    }
    public async Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto updateCategoryDto)
    {
        if (updateCategoryDto.ParentName == " ")
        {
            var categoryParent = new Category
            {
                Id = Guid.NewGuid(),
                Name = updateCategoryDto.Name,
                Type = updateCategoryDto.Type,
                SubCategories = []
            };
            await _categoryRepository.AddNewCategory(categoryParent);
            return new CategoryDto
            {
                Id = categoryParent.Id,
                CategoryName = categoryParent.Name,
                ParentCategoryName = "This is Parent",
                Type = categoryParent.Type,
                SubCategories = categoryParent.SubCategories
            };
        }

        var parentCategory = await _categoryRepository.GetCategoryByName(updateCategoryDto.ParentName);
        if (parentCategory == null)
        {
            throw new Exception(CategoryException);
        }
        var childCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = updateCategoryDto.Name,
            ParentCategoryId = parentCategory.Id,
            Type = updateCategoryDto.Type,
            SubCategories = []
        };

      
        await _categoryRepository.AddNewCategory(childCategory);
        await _categoryRepository.AddSubCategoryAsync(parentCategory, childCategory);
        return new CategoryDto
        {
            Id = childCategory.Id,
            CategoryName = childCategory.Name,
            ParentCategoryName = parentCategory.Name,
            ParentCategoryId = parentCategory.Id,
            Type = childCategory.Type,
        };
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllCategories();
        return categories.Select(cat => new CategoryDto
        {
            Id = cat.Id,
            CategoryName = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            Type = cat.Type,
            SubCategories = cat.SubCategories
        });
    }

    public async Task<CategoryDto> GetCategoryByNameAsync(string name)
    {
        var cat = await _categoryRepository.GetCategoryByName(name);
        if (cat == null)
        {
            throw new Exception(CategoryException);
        }
        if (!cat.Type)
        {
            return new CategoryDto
            {
                Id = cat.Id,
                CategoryName = cat.Name,
                ParentCategoryId = cat.ParentCategoryId,
                Type = cat.Type,
                SubCategories = cat.SubCategories
            };
        }

        return new CategoryDto
        {
            Id = cat.Id,
            CategoryName = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            Type = cat.Type,
            SubCategories = cat.SubCategories
        };
    }

    public async Task<CategoryDto> GetParentCategoryAsync(Guid childId)
    {
        var parent = await _categoryRepository.GetParentByChildId(childId);
        if (parent == null)
        {
            throw new Exception(ParentCategoryException);
        }

        if (!parent.Type)
        {
            throw new Exception("There is no subcategory for a subcategory");
        }
        var child = await _categoryRepository.GetCategoryById(childId);
        return new CategoryDto
        {
            Id = parent.Id,
            CategoryName = parent.Name,
            ParentCategoryId = child.ParentCategoryId,
            Type = parent.Type,
            SubCategories = parent.SubCategories
        };
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
    {
        var cat = await _categoryRepository.GetCategoryById(id);
        if (cat == null)
        {
            throw new Exception(CategoryException);
        }
        return new CategoryDto
        {
            Id = cat.Id,
            CategoryName = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            Type = cat.Type,
            SubCategories = cat.SubCategories
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, AddUpdateCategoryDto updateCategoryDto)
    {
        var targetCategory = await _categoryRepository.GetCategoryById(id);
        if (targetCategory == null)
        {
            throw new Exception(CategoryException);
        }
        targetCategory.Name = updateCategoryDto.Name;
        targetCategory.Type = updateCategoryDto.Type;
       
        if (updateCategoryDto.ParentName == " ")
        {
            await _categoryRepository.UpdateCategory(targetCategory);
            return new CategoryDto
            {
                Id = targetCategory.Id,
                ParentCategoryId =new Guid("00000000-0000-0000-0000-000000000000"),
                ParentCategoryName = "This is a parent",
                Type = targetCategory.Type,
                CategoryName = targetCategory.Name,
                SubCategories = targetCategory.SubCategories
            };
        }
        var newParentCat = await _categoryRepository.GetCategoryByName(updateCategoryDto.ParentName);
        if (newParentCat == null)
        {
            throw new Exception(ParentCategoryException);
        }
        targetCategory.ParentCategoryId = newParentCat.Id;
        await _categoryRepository.AddSubCategoryAsync(newParentCat, targetCategory);
       
        await _categoryRepository.UpdateCategory(targetCategory);
        return new CategoryDto
        {
            Id = targetCategory.Id,
            ParentCategoryId = targetCategory.ParentCategoryId,
            ParentCategoryName = newParentCat.Name,
            Type = targetCategory.Type,
            CategoryName = targetCategory.Name,
            SubCategories = targetCategory.SubCategories
        };
    }

    public async Task<bool> DeleteCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new Exception(CategoryException);
        }
        if (!category.Type)
        {
            await _categoryRepository.DeleteCategoryById(id);
            return true;
        }
        
        if (category.SubCategories.Count() != 0)
        {
            throw new ValidationException("You cannot delete this category before its children!");
        }

        await _categoryRepository.DeleteCategoryById(id);
        return true;
    }
    
}