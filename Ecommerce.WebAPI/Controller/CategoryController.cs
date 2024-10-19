using Ecommerce.Application.Binder.Category;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var cat = await _categoryService.GetCategoryByIdAsync(id);
        var result = new List<object>();
        var subCats = await _categoryService.GetSubCategoriesAsync(cat.Id);
        var categories = new List<object>();
        foreach (var subCat in subCats)
        {
            categories.Add(new
            {
                subCat.Id,
                subCat.CategoryName
            });
        }
            
        result.Add(new
        {
            cat.Id,
            cat.CategoryName,
            cat.ParentCategoryId,
            cat.Type,
            SubCategories = categories,
        });


        return Ok(result);
    }

    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var cats = await _categoryService.GetAllCategoriesAsync();
        var result = new List<object>();
        foreach (var cat in cats)
        {
            var categories = new List<object>();
            var subCats = await _categoryService.GetSubCategoriesAsync(cat.Id);
            // var parent = await _categoryService.GetCategoryByIdAsync(cat.ParentCategoryId);
            foreach (var subCat in subCats)
            {
                categories.Add(new
                {
                    subCat.Id,
                    subCat.CategoryName
                });
            }
            
            result.Add(new
            {
                cat.Id,
                cat.CategoryName,
                cat.ParentCategoryId,
                cat.ParentCategoryName,
                cat.Type,
                SubCategories = categories,
            });
        }
        return Ok(result);
        
    }

    [HttpGet("GetCategoryByName")]
    public async Task<IActionResult> GetCategoryByName(string name)
    {
        var cat = await _categoryService.GetCategoryByNameAsync(name);
        if (!cat.Type)
        {
            return NotFound("There is no subcategories for a subcategory!!!");
        }
        var result = new List<object>();
        var subCats = await _categoryService.GetSubCategoriesAsync(cat.Id);
        var categories = new List<object>();
        foreach (var subCat in subCats)
        {
            categories.Add(new
            {
                subCat.Id,
                subCat.CategoryName
            });
        }
            
        result.Add(new
        {
            cat.Id,
            cat.CategoryName,
            cat.ParentCategoryId,
            cat.Type,
            SubCategories = categories,
        });


        return Ok(result);
    }

    [HttpGet("GetParent/{childId}")]
    public async Task<IActionResult> GetParentByChildId(Guid childId)
    {

        var parent = await _categoryService.GetParentCategoryAsync(childId);
        if (!parent.Type)
        {
            return NotFound("There is no subcategories for a subcategory!!!");
        }
        var result = new List<object>();
        var subCats = await _categoryService.GetSubCategoriesAsync(parent.Id);
        var categories = new List<object>();
        foreach (var subCat in subCats)
        {
            categories.Add(new
            {
                subCat.Id,
                subCat.CategoryName
            });
        }
            
        result.Add(new
        {
            parent.Id,
            ParentName = parent.CategoryName,
            ParentId = parent.ParentCategoryId,
            Type = parent.Type? "Parent Category" : "Subcategory",
            SubCategories = categories,
        });


        return Ok(result);
    }

    [HttpPost("AddNewCategory")]
    public async Task<IActionResult> AddNewCategory([ModelBinder(typeof(CategoryModelBinder))] AddUpdateCategoryDto newUpdateCategory)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { Errors = errors });
        }
        var createdCategory =  await _categoryService.AddCategoryAsync(newUpdateCategory);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }


    [HttpPut("UpdateCategory/{id:guid}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,[ModelBinder(typeof(CategoryModelBinder))] AddUpdateCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { Errors = errors });
        }
        var updateResult = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
        if (updateResult != null)
        {
            return Ok(updateResult);
        }

        return NotFound($"There is no category with ID {id}");
    }

    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategoryById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (!category.Type)
        {
            await _categoryService.DeleteCategoryByIdAsync(id);
            return Ok($"Category {category} has successfully deleted.");
        }

        var subCats = await _categoryService.GetSubCategoriesAsync(id);
        if (subCats.Count()!=0)
        {
            return NotFound("This is a parent category that has children, You cannot delete it before its children.");
        }

        await _categoryService.DeleteCategoryByIdAsync(id);
        return Ok($"Category {category} has successfully deleted.");
    }
    
}