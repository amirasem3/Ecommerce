using System.Text.Json;
using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;

    
        AddUpdateCategoryDto? categoryModel;
        try
        {
            categoryModel = JsonSerializer.Deserialize<AddUpdateCategoryDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("Category", "Invalid JSON format.");
            return;
        }

        if (categoryModel == null)
        {
            bindingContext.ModelState.AddModelError("Category", "Invalid category data.");
            return;
        }
        
        //Category name
        if (string.IsNullOrWhiteSpace(categoryModel.Name))
        {
            bindingContext.ModelState.AddModelError("Category", "Category name is required!");
            return;
        }

        if (categoryModel.Name.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Category", "Category name cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(categoryModel.Name, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Category",  "Invalid characters in category name!");
            return;
        }

        try
        {
            await _categoryServices.GetCategoryByNameAsync(categoryModel.Name);
            bindingContext.ModelState.AddModelError("Category", "This category is already exists!");
        }
        catch (Exception e)
        {
            if (e.Message!= CategoryServices.CategoryException && e.Message!=CategoryServices.ParentCategoryException)
            {
                bindingContext.ModelState.AddModelError("Category","An unexpected error occurred while checking the name uniqueness.");
            }
        }

        //Parent category name
        if (string.IsNullOrEmpty(categoryModel.ParentName))
        {
            bindingContext.ModelState.AddModelError("Category", "Parent category name is required!");
            bindingContext.ModelState.AddModelError("Category", "HINT: For parent category put a whitespace");
            return;
        }
        if (!Regex.Match(categoryModel.ParentName, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Category",  "Invalid characters in category parent name!");
            return;
        }

        if (categoryModel.Type.GetType() != typeof(bool))
        {
            bindingContext.ModelState.AddModelError("Category", "Enter a valid type value!");
            return;
        }

        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;
        
        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: categoryModel);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }
        
        bindingContext.Result = ModelBindingResult.Success(categoryModel);
    }
}