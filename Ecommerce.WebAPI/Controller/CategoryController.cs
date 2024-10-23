using Ecommerce.Application.Binder.Category;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryServices _categoryService;

    public CategoryController(CategoryServices categoryService)
    {
        _categoryService = categoryService;
    }

    // [HttpGet("GetCategoryById/{id}")]
    // public async Task<IActionResult> GetCategoryById(Guid id)
    // {
    //     try
    //     {
    //         var cat = await _categoryService.GetCategoryByIdAsync(id);
    //         return Ok(cat);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        try
        {
            var cat = await _categoryService.GetCategoryById(id);
            return Ok(cat);
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }

    // [HttpGet("GetAllCategories")]
    // public async Task<IActionResult> GetAllCategories()
    // {
    //     try
    //     {
    //         var cats = await _categoryService.GetAllCategoriesAsync();
    //         return Ok(cats);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            return Ok(cats);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // [HttpGet("GetCategoryByName")]
    // public async Task<IActionResult> GetCategoryByName(string name)
    // {
    //     try
    //     {
    //         var cat = await _categoryService.GetCategoryByNameAsync(name);
    //         return Ok(cat);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpGet("GetCategoryByName")]
    public async Task<IActionResult> GetCategoryByName(string name)
    {
        try
        {
            var cat = await _categoryService.GetCategoryByNameAsync(name);
            return Ok(cat);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // [HttpGet("GetParent/{childId}")]
    // public async Task<IActionResult> GetParentByChildId(Guid childId)
    // {
    //     try
    //     {
    //         var parent = await _categoryService.GetParentCategoryAsync(childId);
    //         return Ok(parent);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpGet("GetParent/{childId}")]
    public async Task<IActionResult> GetParentByChildId(Guid childId)
    {
        try
        {
            var parent = await _categoryService.GetParentCategoryAsync(childId);
            return Ok(parent);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // [HttpPost("AddNewCategory")]
    // [Consumes("application/json")]
    // public async Task<IActionResult> AddNewCategory([FromBody] AddUpdateCategoryDto newCategory)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
    //     }
    //
    //     try
    //     {
    //         var createdCategory = await _categoryService.AddCategoryAsync(newCategory);
    //         return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }

    
    [HttpPost("AddNewCategory")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddNewCategory([FromBody] AddUpdateCategoryDto newCategory)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var createdCategory = await _categoryService.InsertCategoryAsync(newCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    //
    // [HttpPut("UpdateCategory/{id:guid}")]
    // public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
    //     [ModelBinder(typeof(CategoryModelBinder))] AddUpdateCategoryDto updateCategoryDto)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
    //     }
    //
    //     try
    //     {
    //         var updateResult = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
    //         return Ok(updateResult);
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpPut("UpdateCategory/{id:guid}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
        [FromBody] AddUpdateCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updateResult = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            return Ok(updateResult);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // [HttpDelete("DeleteCategory/{id}")]
    // public async Task<IActionResult> DeleteCategoryById(Guid id)
    // {
    //
    //
    //     try
    //     {
    //         await _categoryService.DeleteCategoryByIdAsync(id);
    //         return Ok($"Category with ID {id} has successfully deleted.");
    //     }
    //     catch (Exception e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategoryByIdTest(Guid id)
    {


        try
        {
            await _categoryService.DeleteCategoryByIdAsync(id);
            return Ok($"Category with ID {id} has successfully deleted.");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}