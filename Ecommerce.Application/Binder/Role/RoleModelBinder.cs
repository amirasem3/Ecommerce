using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecommerce.Application.Binder;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class RoleModelBinder : IModelBinder
{
    private readonly RoleService _roleService;

    public RoleModelBinder(RoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;

    
        AddUpdateRoleDto? addUpdateRoleDto;
        try
        {
            addUpdateRoleDto = JsonSerializer.Deserialize<AddUpdateRoleDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("Role", "Invalid JSON format.");
            return;
        }

        if (addUpdateRoleDto == null)
        {
            bindingContext.ModelState.AddModelError("Role", "Invalid invoice data.");
            return;
        }
        

        if (string.IsNullOrWhiteSpace(addUpdateRoleDto.Name))
        {
            bindingContext.ModelState.AddModelError("Role", "Name is required.");
        }

        if (!Regex.Match(addUpdateRoleDto.Name, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Role", "Invalid character in name");
        }


        try
        {
            var roleExist = await _roleService.GetRoleByNameAsync(addUpdateRoleDto.Name);
            if (roleExist != null)
            {
                bindingContext.ModelState.AddModelError("Role", "Role name already exists");
            }
        }
        catch (Exception e)
        {
            if (e.Message!=RoleService.RoleException)
            {
                bindingContext.ModelState.AddModelError("Role", "An unexpected error occurred while checking the name uniqueness.");
            }
        }
       

        if (addUpdateRoleDto.Name.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Role", "Name cannot exceed 40 characters.");
        }
        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;
        
        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: addUpdateRoleDto);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(addUpdateRoleDto);
        
    }
}