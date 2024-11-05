using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.Category;

public class CategoryModelBinderProvider : IModelBinderProvider
{
    private readonly CategoryService _categoryService;

    public CategoryModelBinderProvider(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateCategoryDto))
        {
            return new CategoryModelBinder(_categoryService);
        }

        return null;
    }
}