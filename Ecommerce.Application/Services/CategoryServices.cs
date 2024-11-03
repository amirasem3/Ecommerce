using System.ComponentModel.DataAnnotations;
using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;

namespace Ecommerce.Application.Services;

public class CategoryServices
{
    public const string CategoryException = "Category Not Found!";
    public const string ParentCategoryException = "Parent Category Not Found!";
    private readonly UnitOfWork _unitOfWork;

    public CategoryServices(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> GetCategoryById(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a category by ID", args: [id]);
        var category =
            await _unitOfWork.CategoryRepository.GetByIdAsync(id,
                "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");

        if (category == null)
        {
            LoggerHelper.LogWithDetails("There is no category with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(CategoryException);
        }


        if (category.IsParentChild() || (!category.IsParent() && !category.IsParentChild()))
        {
            var parentInParent =
                await _unitOfWork.CategoryRepository.GetByIdAsync(category.ParentCategoryId!, "SubCategories");
            var resultCategory = new CategoryDto(category.IsParent())
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = (Guid)category.ParentCategoryId!,
                SubCategories = category.SubCategories,
                ParentCategoryName = parentInParent.Name
            };
            LoggerHelper.LogWithDetails("Target Category Found", args: [id], retrievedData: resultCategory);
            return resultCategory;
        }

        var resCat = new CategoryDto(category.IsParent())
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryName = "Root",
            SubCategories = category.SubCategories
        };
        LoggerHelper.LogWithDetails("Target Category Found", args: [id], retrievedData: resCat);
        return resCat;
    }

    public async Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto newCategory)
    {
        var parentRequested = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            "SubCategories", uniquePropertyValue: newCategory.ParentName);
        if (parentRequested == null && newCategory.ParentName != "Root")
        {
            LoggerHelper.LogWithDetails("Invalid Parent Category Name", args: [newCategory.ParentName],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ParentCategoryException);
        }

        if (newCategory.TypeString == "Parent" && newCategory.ParentName == "Root")
        {
            var categoryParent = new Category
            {
                Id = Guid.NewGuid(),
                Name = newCategory.Name,
                TypeString = newCategory.TypeString,
                ParentCategoryId = Guid.Empty,
                SubCategories = []
            };
            await _unitOfWork.CategoryRepository.InsertAsync(categoryParent);
            await _unitOfWork.SaveAsync();
            var resCat1 = new CategoryDto(categoryParent.IsParent())
            {
                Id = categoryParent.Id,
                Name = categoryParent.Name,
                SubCategories = categoryParent.SubCategories,
                ParentCategoryName = "Root"
            };
            LoggerHelper.LogWithDetails("New Parent category added successfully.", args: [newCategory],
                retrievedData: resCat1);
            return resCat1;
        }

        if (newCategory.TypeString == "Parent" && newCategory.ParentName != "Root")
        {
            LoggerHelper.LogWithDetails("Attempt to add a Parent subcategory");
            var categoryParentSubParent = new Category()
            {
                Id = Guid.NewGuid(),
                Name = newCategory.Name,
                TypeString = newCategory.TypeString,
                SubCategories = [],
                ParentCategoryId = parentRequested!.Id
            };
            await _unitOfWork.CategoryRepository.InsertAsync(categoryParentSubParent);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails("New Parent subcategory added successfully.",
                retrievedData: categoryParentSubParent);
            parentRequested.SubCategories.Add(categoryParentSubParent);
            LoggerHelper.LogWithDetails($"New parent subcategory added to a {newCategory.ParentName}'s subcategories",
                args: [categoryParentSubParent], retrievedData: parentRequested);
            _unitOfWork.CategoryRepository.Update(parentRequested);
            await _unitOfWork.SaveAsync();

            var resCat2 = new CategoryDto(newCategory.TypeString == "Parent" && parentRequested != null)
            {
                Id = categoryParentSubParent.Id,
                Name = categoryParentSubParent.Name,
                SubCategories = categoryParentSubParent.SubCategories,
                ParentCategoryId = (Guid)categoryParentSubParent.ParentCategoryId,
                ParentCategoryName = parentRequested!.Name
            };
            LoggerHelper.LogWithDetails("New Parent category added successfully.", args: [newCategory],
                retrievedData: resCat2);

            return resCat2;
        }

        LoggerHelper.LogWithDetails("Attempt to add a child category");
        var childCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = newCategory.Name,
            TypeString = newCategory.TypeString,
            ParentCategoryId = parentRequested!.Id,
            SubCategories = []
        };
        await _unitOfWork.CategoryRepository.InsertAsync(childCategory);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("New Child category added successfully.");
        parentRequested.SubCategories.Add(childCategory);
        _unitOfWork.CategoryRepository.Update(parentRequested);

        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails($"New parent subcategory added to a {newCategory.ParentName}'s subcategories",
            args: [childCategory], retrievedData: parentRequested);
        var resCat3 = new CategoryDto(childCategory.IsParent())
        {
            Id = childCategory.Id,
            Name = childCategory.Name,
            ParentCategoryName = parentRequested.Name,
            ParentCategoryId = parentRequested.Id,
        };
        LoggerHelper.LogWithDetails("New Child Added successfully", args: [newCategory], retrievedData: childCategory);
        return resCat3;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        LoggerHelper.LogWithDetails("Attempt to get all categories");
        var categories =
            await _unitOfWork.CategoryRepository.GetAsync(
                includeProperties: "SubCategories,SubCategories.SubCategories");
        if (categories == null)
        {
            LoggerHelper.LogWithDetails("There is no category.", logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Category table is empty.");
        }

        List<CategoryDto> allCategories = new List<CategoryDto>();
        foreach (var category in categories)
        {
            if (category.IsParentChild() || (!category.IsParent() && !category.IsParentChild()))
            {
                var parentInParent =
                    await _unitOfWork.CategoryRepository.GetByIdAsync(category.ParentCategoryId!, "SubCategories");

                allCategories.Add(new CategoryDto(category.IsParent())
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentCategoryId = (Guid)category.ParentCategoryId!,
                    SubCategories = category.SubCategories,
                    ParentCategoryName = parentInParent.Name
                });
            }
            else
            {
                allCategories.Add(new CategoryDto(category.IsParent())
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentCategoryName = "Root",
                    SubCategories = category.SubCategories
                });
            }
        }

        LoggerHelper.LogWithDetails("All Categories", retrievedData: allCategories);
        return allCategories;
    }


    public async Task<CategoryDto> GetCategoryByNameAsync(string name)
    {
        var cat = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            includeProperties: "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories",
            uniquePropertyValue: name);
        if (cat == null)
        {
            LoggerHelper.LogWithDetails("There is no category with this name", args: [name],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(CategoryException);
        }


        if (cat.IsParentChild() || (!cat.IsParent() && !cat.IsParentChild()))
        {
            var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(cat.ParentCategoryId!, "SubCategories");
            var catResult1 = new CategoryDto(cat.IsParent())
            {
                Id = cat.Id,
                Name = cat.Name,
                ParentCategoryId = (Guid)cat.ParentCategoryId!,
                SubCategories = cat.SubCategories,
                ParentCategoryName = parent.Name
            };
            LoggerHelper.LogWithDetails("Target Category Found", catResult1);
            return catResult1;
        }

        var catResult2 = new CategoryDto(cat.IsParent())
        {
            Id = cat.Id,
            Name = cat.Name,
            ParentCategoryName = "Root",
            SubCategories = cat.SubCategories
        };
        LoggerHelper.LogWithDetails("Target Result Found", args: [name], retrievedData: catResult2);
        return catResult2;
    }


    public async Task<CategoryDto> GetParentCategoryAsync(Guid childId)
    {
        LoggerHelper.LogWithDetails("Attempt to get a parent category by its child's ID", args: [childId]);
        var child = await _unitOfWork.CategoryRepository.GetByIdAsync(childId,
            "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");
        if (child == null)
        {
            LoggerHelper.LogWithDetails("There is no child wit this ID", args: [childId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(CategoryException);
        }

        if (child.IsParent() && !child.IsParentChild())
        {
            LoggerHelper.LogWithDetails("There is no child wit this ID", args: [childId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("There is no child category with this ID.");
        }

        var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(child.ParentCategoryId!,
            includeProperties: "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");
        LoggerHelper.LogWithDetails("Getting the parent", args: [childId], retrievedData: parent);

        if (parent.IsParentChild())
        {
            var parentInParent = await _unitOfWork.CategoryRepository.GetByIdAsync(parent.ParentCategoryId!);
            var resCat1 = new CategoryDto(!parent.IsParent())
            {
                Id = parent.Id,
                Name = parent.Name,
                ParentCategoryName = parentInParent.Name,
                ParentCategoryId = parentInParent.Id,
                SubCategories = parent.SubCategories
            };
            LoggerHelper.LogWithDetails("Target Parent Found", args: [childId], retrievedData: resCat1);
            return resCat1;
        }

        var resCat2 = new CategoryDto(parent.IsParent())
        {
            Id = parent.Id,
            Name = parent.Name,
            ParentCategoryName = "Root",
            SubCategories = parent.SubCategories
        };
        LoggerHelper.LogWithDetails("Target Parent Found", args: [childId], retrievedData: resCat2);
        return resCat2;
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, AddUpdateCategoryDto updateCategoryDto)
    {
        LoggerHelper.LogWithDetails("Attempt to update a category", args: [id, updateCategoryDto]);
        var targetCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id,
            includeProperties: "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");
        if (targetCategory == null)
        {
            LoggerHelper.LogWithDetails("There is no category with this id", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(CategoryException);
        }

        targetCategory.Name = updateCategoryDto.Name;
        targetCategory.TypeString = updateCategoryDto.TypeString;

        if (!targetCategory.IsParent() && !targetCategory.IsParentChild() && updateCategoryDto.TypeString == "Parent")
        {
            var parentInParent =
                await _unitOfWork.CategoryRepository.GetByIdAsync(targetCategory.ParentCategoryId!,
                    "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");
            if (parentInParent.Name != updateCategoryDto.ParentName)
            {
                LoggerHelper.LogWithDetails("Attempt to update the category's parent",
                    args: [updateCategoryDto.ParentName]);
                var newParentCatChild = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(
                    uniqueProperty: "Name",
                    includeProperties:
                    "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories",
                    uniquePropertyValue: updateCategoryDto.ParentName);
                targetCategory.ParentCategoryId = newParentCatChild.Id;

                _unitOfWork.CategoryRepository.Update(targetCategory);
                await _unitOfWork.SaveAsync();
                var resCat1 = new CategoryDto(!targetCategory.IsParent() && !targetCategory.IsParentChild())
                {
                    Id = targetCategory.Id,
                    Name = targetCategory.Name,
                    ParentCategoryId = (Guid)targetCategory.ParentCategoryId!,
                    SubCategories = targetCategory.SubCategories,
                    ParentCategoryName = newParentCatChild.Name
                };
                LoggerHelper.LogWithDetails($"{updateCategoryDto.Name} updated successfully.",
                    args: [updateCategoryDto], retrievedData: resCat1);
                return resCat1;
            }

            LoggerHelper.LogWithDetails("Attempt to update a category without changing the parent");
            _unitOfWork.CategoryRepository.Update(targetCategory);
            await _unitOfWork.SaveAsync();
            var resCat2 = new CategoryDto(targetCategory.IsParent())
            {
                Id = targetCategory.Id,
                Name = targetCategory.Name,
                ParentCategoryId = (Guid)targetCategory.ParentCategoryId!,
                SubCategories = targetCategory.SubCategories,
                ParentCategoryName = parentInParent.Name
            };
            LoggerHelper.LogWithDetails($"{updateCategoryDto.Name} updated successfully.",
                args: [updateCategoryDto], retrievedData: resCat2);
            return resCat2;
        }

        if (targetCategory.IsParent() && !targetCategory.IsParentChild())
        {
            LoggerHelper.LogWithDetails("Attempt to update a parent category that has parent category");
            var newCategory = new Category()
            {
                Id = Guid.NewGuid(),
                Name = targetCategory.Name,
                TypeString = targetCategory.TypeString,
                ParentCategoryId = Guid.Empty,
                SubCategories = targetCategory.SubCategories
            };
            _unitOfWork.CategoryRepository.Update(targetCategory);
            await _unitOfWork.SaveAsync();
            var resCat3 = new CategoryDto(!targetCategory.IsParent())
            {
                Id = newCategory.Id,
                ParentCategoryId = (Guid)newCategory.ParentCategoryId!,
                Name = newCategory.Name,
                SubCategories = targetCategory.SubCategories
            };
            LoggerHelper.LogWithDetails($"{updateCategoryDto.Name} updated successfully.",
                args: [updateCategoryDto], retrievedData: resCat3);
            return resCat3;
        }

        LoggerHelper.LogWithDetails("Attempt to update a normal child category");
        var newParentCat = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            includeProperties: "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories",
            uniquePropertyValue: updateCategoryDto.ParentName);

        targetCategory.ParentCategoryId = newParentCat.Id;
        newParentCat.SubCategories.Add(targetCategory);
        _unitOfWork.CategoryRepository.Update(newParentCat);
        await _unitOfWork.SaveAsync();

        _unitOfWork.CategoryRepository.Update(targetCategory);
        await _unitOfWork.SaveAsync();
        var resCat4 = new CategoryDto(targetCategory.IsParent())
        {
            Id = targetCategory.Id,
            ParentCategoryId = (Guid)targetCategory.ParentCategoryId,
            ParentCategoryName = newParentCat.Name,
            Name = targetCategory.Name,
            SubCategories = targetCategory.SubCategories
        };
        LoggerHelper.LogWithDetails($"{updateCategoryDto.Name} updated successfully.",
            args: [updateCategoryDto], retrievedData: resCat4);
        return resCat4;
    }


    public async Task DeleteCategoryByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to delete a category", args: [id]);
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id,
            includeProperties: "SubCategories,SubCategories.SubCategories,SubCategories.SubCategories.SubCategories");
        if (category == null)
        {
            LoggerHelper.LogWithDetails("There is no category with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(CategoryException);
        }

        if (!category.IsParent())
        {
            await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails("Successful child Delete.", args: [id], retrievedData: category);
            return;
        }

        if (category.SubCategories.Count() != 0)
        {
            LoggerHelper.LogWithDetails("You cannot delete this category before its children!",
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ValidationException("You cannot delete this category before its children!");
        }

        await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Parent Delete",args:[id],retrievedData:category);
    }
}