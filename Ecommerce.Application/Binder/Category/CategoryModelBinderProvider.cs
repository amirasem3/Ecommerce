using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.Category;

public class CategoryModelBinderProvider : IModelBinderProvider
{
    private readonly CategoryServices _categoryServices;

    public CategoryModelBinderProvider(CategoryServices categoryServices)
    {
        _categoryServices = categoryServices;
    }

public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateCategoryDto))
        {
            return new CategoryModelBinder(_categoryServices);
        }

        return null;
    }
}