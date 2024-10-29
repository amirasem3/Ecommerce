using System.ComponentModel.DataAnnotations;
using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
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
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, "SubCategories");

        if (category == null)
        {
            throw new Exception(CategoryException);
        }


        if (category.IsParentChild() || (!category.IsParent() && !category.IsParentChild()))
        {
            var parentInParent =
                await _unitOfWork.CategoryRepository.GetByIdAsync(category.ParentCategoryId!, "SubCategories");
            if (parentInParent == null)
            {
                throw new Exception(ParentCategoryException);
            }

            Console.WriteLine(parentInParent.Name);
            return new CategoryDto(category.IsParent())
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = (Guid)category.ParentCategoryId!,
                SubCategories = category.SubCategories,
                ParentCategoryName = parentInParent.Name
            };
        }

        return new CategoryDto(category.IsParent())
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryName = "Root",
            SubCategories = category.SubCategories
        };
    }

    public async Task<CategoryDto> InsertCategoryAsync(AddUpdateCategoryDto newCategory)
    {
        var parentRequested = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            "SubCategories", uniquePropertyValue: newCategory.ParentName);
        if (parentRequested == null && newCategory.ParentName != "Root")
        {
            throw new Exception(ParentCategoryException);
        }
        if (newCategory.TypeString == "Parent" && newCategory.ParentName=="Root")
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
            return new CategoryDto(categoryParent.IsParent())
            {
                Id = categoryParent.Id,
                Name = categoryParent.Name,
                SubCategories = categoryParent.SubCategories,
                ParentCategoryName = "Root"
            };
        }

        if(newCategory.TypeString == "Parent" && newCategory.ParentName!="Root")
        {
            var categoryParentSubParent = new Category()
            {
                Id = Guid.NewGuid(),
                Name = newCategory.Name,
                TypeString = newCategory.TypeString,
                SubCategories = [],
                ParentCategoryId = parentRequested.Id
            };
            await _unitOfWork.CategoryRepository.InsertAsync(categoryParentSubParent);
            await _unitOfWork.SaveAsync();
            
            parentRequested.SubCategories.Add(categoryParentSubParent);
            _unitOfWork.CategoryRepository.Update(parentRequested);
            await _unitOfWork.SaveAsync();

            return new CategoryDto(newCategory.TypeString == "Parent" && parentRequested != null)
            {
                Id = categoryParentSubParent.Id,
                Name = categoryParentSubParent.Name,
                SubCategories = categoryParentSubParent.SubCategories,
                ParentCategoryId = (Guid)categoryParentSubParent.ParentCategoryId,
                ParentCategoryName = parentRequested!.Name
            };
        }
        var childCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = newCategory.Name,
            TypeString = newCategory.TypeString,
            ParentCategoryId = parentRequested.Id,
            SubCategories = []
        };
        await _unitOfWork.CategoryRepository.InsertAsync(childCategory);
        parentRequested.SubCategories.Add(childCategory);
        _unitOfWork.CategoryRepository.Update(parentRequested);
        await _unitOfWork.SaveAsync();

        return new CategoryDto(childCategory.IsParent())
        {
            Id = childCategory.Id,
            Name = childCategory.Name,
            ParentCategoryName = parentRequested.Name,
            ParentCategoryId = parentRequested.Id,
        };
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAsync(includeProperties: "SubCategories");
        List<CategoryDto> allCategories = new List<CategoryDto>();
        foreach (var category in categories)
        {
            if (category.IsParentChild()  || (!category.IsParent() && !category.IsParentChild()))
            {
                var parentInParent =
                    await _unitOfWork.CategoryRepository.GetByIdAsync(category.ParentCategoryId!, "SubCategories");
                if (parentInParent == null)
                {
                    throw new Exception(ParentCategoryException);
                }

                Console.WriteLine(parentInParent.Name);
                allCategories.Add(new CategoryDto(category.IsParent())
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentCategoryId = (Guid)category.ParentCategoryId!,
                    SubCategories = category.SubCategories,
                    ParentCategoryName = parentInParent.Name
                });
                // continue;
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

        return allCategories;
    }


    public async Task<CategoryDto> GetCategoryByNameAsync(string name)
    {
        var cat = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            includeProperties: "SubCategories", uniquePropertyValue: name);
        if (cat == null)
        {
            throw new Exception(CategoryException);
        }


        if (cat.IsParentChild()  || (!cat.IsParent() && !cat.IsParentChild()))
        {
            var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(cat.ParentCategoryId!, "SubCategories");
            if (parent == null)
            {
                throw new Exception(ParentCategoryException);
            }

            Console.WriteLine(parent.Name);
            return new CategoryDto(cat.IsParent())
            {
                Id = cat.Id,
                Name = cat.Name,
                ParentCategoryId = (Guid)cat.ParentCategoryId!,
                SubCategories = cat.SubCategories,
                ParentCategoryName = parent.Name
            };
        }

        return new CategoryDto(cat.IsParent())
        {
            Id = cat.Id,
            Name = cat.Name,
            ParentCategoryName = "Root",
            SubCategories = cat.SubCategories
        };
    }


    public async Task<CategoryDto> GetParentCategoryAsync(Guid childId)
    {
        var child = await _unitOfWork.CategoryRepository.GetByIdAsync(childId, "SubCategories");
        if (child == null)
        {
            throw new Exception(CategoryException);
        }

        if (child.IsParent() && !child.IsParentChild())
        {
            throw new Exception("There is no child category with this ID.");
        }

        var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(child.ParentCategoryId!);
        if (parent == null)
        {
            throw new Exception(ParentCategoryException);
        }
        

        if (parent.IsParentChild())
        {
            var parentInParent = await _unitOfWork.CategoryRepository.GetByIdAsync(parent.ParentCategoryId!);
            return new CategoryDto(!parent.IsParent())
            {
                Id = parent.Id,
                Name = parent.Name,
                ParentCategoryName = parentInParent.Name,
                ParentCategoryId = parentInParent.Id,
                SubCategories = parent.SubCategories
            };
        }

        return new CategoryDto(parent.IsParent())
        {
            Id = parent.Id,
            Name = parent.Name,
            ParentCategoryName = "Root",
            SubCategories = parent.SubCategories
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, AddUpdateCategoryDto updateCategoryDto)
    {
        var targetCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id, "SubCategories");
        if (targetCategory == null)
        {
            throw new Exception(CategoryException);
        }

        targetCategory.Name = updateCategoryDto.Name;
        targetCategory.TypeString = updateCategoryDto.TypeString;

        if (!targetCategory.IsParent() && !targetCategory.IsParentChild() && updateCategoryDto.TypeString == "Parent")
        {
            var parentInParent =
                await _unitOfWork.CategoryRepository.GetByIdAsync(targetCategory.ParentCategoryId!, "SubCategories");
            if (parentInParent == null)
            {
                throw new Exception(ParentCategoryException);
            }

            if (parentInParent.Name != updateCategoryDto.ParentName)
            {
                var newParentCatChild = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(
                    uniqueProperty: "Name",
                    includeProperties: "SubCategories", uniquePropertyValue: updateCategoryDto.ParentName);
                targetCategory.ParentCategoryId = newParentCatChild.Id;

                _unitOfWork.CategoryRepository.Update(targetCategory);
                await _unitOfWork.SaveAsync();
                return new CategoryDto(!targetCategory.IsParent() && !targetCategory.IsParentChild())
                {
                    Id = targetCategory.Id,
                    Name = targetCategory.Name,
                    ParentCategoryId = (Guid)targetCategory.ParentCategoryId!,
                    SubCategories = targetCategory.SubCategories,
                    ParentCategoryName = newParentCatChild.Name
                };
            }

            _unitOfWork.CategoryRepository.Update(targetCategory);
            await _unitOfWork.SaveAsync();
            return new CategoryDto(targetCategory.IsParent())
            {
                Id = targetCategory.Id,
                Name = targetCategory.Name,
                ParentCategoryId = (Guid)targetCategory.ParentCategoryId!,
                SubCategories = targetCategory.SubCategories,
                ParentCategoryName = parentInParent.Name
            };
        }

        if (targetCategory.IsParent() && !targetCategory.IsParentChild())
        {
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
            return new CategoryDto(!targetCategory.IsParent())
            {
                Id = newCategory.Id,
                ParentCategoryId = (Guid)newCategory.ParentCategoryId!,
                Name = newCategory.Name,
                SubCategories = targetCategory.SubCategories
            };
        }

        var newParentCat = await _unitOfWork.CategoryRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            includeProperties: "SubCategories", uniquePropertyValue: updateCategoryDto.ParentName);
        if (newParentCat == null)
        {
            throw new Exception(ParentCategoryException);
        }

        targetCategory.ParentCategoryId = newParentCat.Id;
        newParentCat.SubCategories.Add(targetCategory);
        _unitOfWork.CategoryRepository.Update(newParentCat);
        await _unitOfWork.SaveAsync();

        _unitOfWork.CategoryRepository.Update(targetCategory);
        await _unitOfWork.SaveAsync();
        return new CategoryDto(targetCategory.IsParent())
        {
            Id = targetCategory.Id,
            ParentCategoryId = (Guid)targetCategory.ParentCategoryId,
            ParentCategoryName = newParentCat.Name,
            Name = targetCategory.Name,
            SubCategories = targetCategory.SubCategories
        };
    }


    public async Task DeleteCategoryByIdAsync(Guid id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new Exception(CategoryException);
        }

        if (!category.IsParent())
        {
            await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            return;
        }

        if (category.SubCategories.Count() != 0)
        {
            throw new ValidationException("You cannot delete this category before its children!");
        }

        await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }
}