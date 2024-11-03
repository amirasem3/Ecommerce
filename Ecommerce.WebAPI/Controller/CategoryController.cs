﻿using Ecommerce.Application.Binder.Category;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
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


    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        LoggerHelper.LogWithDetails(args: [id]);
        try
        {
            var cat = await _categoryService.GetCategoryById(id);
            return Ok(cat);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect category ID.", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e);
        }
    }

    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        LoggerHelper.LogWithDetails();
        try
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            return Ok(cats);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Error", retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetCategoryByName")]
    public async Task<IActionResult> GetCategoryByName(string name)
    {
        LoggerHelper.LogWithDetails(args: [name]);
        try
        {
            var cat = await _categoryService.GetCategoryByNameAsync(name);
            return Ok(cat);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Name", retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetParent/{childId}")]
    public async Task<IActionResult> GetParentByChildId(Guid childId)
    {
        LoggerHelper.LogWithDetails(args: [childId]);
        try
        {
            var parent = await _categoryService.GetParentCategoryAsync(childId);
            return Ok(parent);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Invalid Child ID", args: [childId], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddNewCategory")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddNewCategory([FromBody] AddUpdateCategoryDto newCategory)
    {
        LoggerHelper.LogWithDetails(args: [newCategory]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [newCategory],
                retrievedData: ModelState["Category"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var createdCategory = await _categoryService.AddCategoryAsync(newCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", args: [newCategory], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPut("UpdateCategory/{id:guid}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
        [FromBody] AddUpdateCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [updateCategoryDto],
                retrievedData: ModelState["Category"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Category"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updateResult = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            return Ok(updateResult);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", args: [updateCategoryDto], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }


    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategoryById(Guid id)
    {
        LoggerHelper.LogWithDetails(args: [id]);
        try
        {
            await _categoryService.DeleteCategoryByIdAsync(id);
            return Ok($"Category with ID {id} has successfully deleted.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Error", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}