using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.Category;

public class CategoryModelBinder : IModelBinder
{
    private readonly CategoryServices _categoryServices;

    public CategoryModelBinder(CategoryServices categoryServices)
    {
        _categoryServices = categoryServices;
    }
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var categoryNameValue = bindingContext.ValueProvider.GetValue("categoryName").FirstValue;
        var parentCategoryNameValue = bindingContext.ValueProvider.GetValue("parentCategoryName").FirstValue;
        var typeValue = bindingContext.ValueProvider.GetValue("type").FirstValue;

        
        //Category name
        if (string.IsNullOrWhiteSpace(categoryNameValue))
        {
            bindingContext.ModelState.AddModelError("categoryName", "Category name is required!");
            return;
        }

        if (categoryNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("categoryName", "Category name cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(categoryNameValue, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("categoryName",  "Invalid characters in category name!");
            return;
        }

        //Parent category name
        if (string.IsNullOrEmpty(parentCategoryNameValue))
        {
            bindingContext.ModelState.AddModelError("parentCategoryName", "Parent category name is required!");
            bindingContext.ModelState.AddModelError("parentCategoryName", "HINT: For parent category put a whitespace");
            return;
        }
        if (!Regex.Match(parentCategoryNameValue, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("parentCategoryName",  "Invalid characters in category parent name!");
            return;
        }

        if (!bool.TryParse(typeValue, out bool type))
        {
            bindingContext.ModelState.AddModelError("type", "Enter a valid type value!");
            return;
        }

        var category = new AddUpdateCategoryDto()
        {
            CategoryName = categoryNameValue,
            ParentCategoryName = parentCategoryNameValue,
            Type = type
        };
        
        bindingContext.Result = ModelBindingResult.Success(category);
    }
}