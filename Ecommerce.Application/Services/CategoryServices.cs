using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Ecommerce.Application.Services;

public class CategoryServices : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryServices(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto updateCategoryDto)
    {
        if (updateCategoryDto.ParentCategoryName == " ")
        {
            var categoryParent = new Category
            {
                Id = Guid.NewGuid(),
                Name = updateCategoryDto.CategoryName,
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

        var parentCategory = await _categoryRepository.GetCategoryByName(updateCategoryDto.ParentCategoryName);
        var childCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = updateCategoryDto.CategoryName,
            ParentCategoryId = parentCategory.Id,
            Type = updateCategoryDto.Type,
        };
        await _categoryRepository.AddNewCategory(childCategory);
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
        if (!cat.Type)
        {
            return new CategoryDto
            {
                Id = cat.Id,
                CategoryName = cat.Name,
                ParentCategoryId = cat.ParentCategoryId,
                Type = cat.Type,
            };
        }

        return new CategoryDto
        {
            Id = cat.Id,
            CategoryName = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            Type = cat.Type,
        };
    }

    public async Task<CategoryDto> GetParentCategoryAsync(Guid childId)
    {
        var parent = await _categoryRepository.GetParentByChildId(childId);
        var child = await _categoryRepository.GetCategoryById(childId);
        return new CategoryDto
        {
            Id = parent.Id,
            CategoryName = parent.Name,
            ParentCategoryId = (Guid)child.ParentCategoryId!,
            Type = parent.Type,
        };
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
    {
        var cat = await _categoryRepository.GetCategoryById(id);
        var cats = await _categoryRepository.GetAllCategories();
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
        targetCategory.Name = updateCategoryDto.CategoryName;
        targetCategory.Type = updateCategoryDto.Type;
       
        if (updateCategoryDto.ParentCategoryName == " ")
        {
            await _categoryRepository.UpdateCategory(targetCategory);
            return new CategoryDto
            {
                Id = targetCategory.Id,
                ParentCategoryId =new Guid("00000000-0000-0000-0000-000000000000"),
                ParentCategoryName = "This is a parent",
                Type = targetCategory.Type,
                CategoryName = targetCategory.Name
            };
        }
        var newParentCat = await _categoryRepository.GetCategoryByName(updateCategoryDto.ParentCategoryName);
        targetCategory.ParentCategoryId = newParentCat.Id;
       
        await _categoryRepository.UpdateCategory(targetCategory);
        return new CategoryDto
        {
            Id = targetCategory.Id,
            ParentCategoryId = targetCategory.ParentCategoryId,
            ParentCategoryName = newParentCat.Name,
            Type = targetCategory.Type,
            CategoryName = targetCategory.Name
        };
    }

    public async Task<bool> DeleteCategoryById(Guid id)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (!category.Type)
        {
            await _categoryRepository.DeleteCategoryById(id);
            return true;
        }


        var subCategories = await _categoryRepository.GetSubCategory(id);
        if (subCategories.Count() != 0)
        {
            return false;
        }

        await _categoryRepository.DeleteCategoryById(id);
        return true;
    }

    public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(Guid parentCategory)
    {
        var categories = await _categoryRepository.GetSubCategory(parentCategory);
        return categories.Select(cat => new CategoryDto
        {
            Id = cat.Id,
            CategoryName = cat.Name,
            ParentCategoryId = (Guid)cat.ParentCategoryId!,
            Type = cat.Type
        });
    }
}