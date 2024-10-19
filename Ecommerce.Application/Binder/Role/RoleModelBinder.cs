using Ecommerce.Application.Binder;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class RoleModelBinder : IModelBinder
{
    private readonly RoleService _roleService;

    public RoleModelBinder(RoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var nameValue = bindingContext.ValueProvider.GetValue("name").FirstValue;

        if (string.IsNullOrWhiteSpace(nameValue))
        {
            // Add error to ModelState
            bindingContext.ModelState.AddModelError("Name", "Name is required.");
            return;
        }

        var roleExist = await _roleService.GetRoleByNameAsync(nameValue);
        if (roleExist != null)
        {
            bindingContext.ModelState.AddModelError("Name", "Role name already exists");
        }

        if (nameValue.Length > 40)
        {
            // Add error to ModelState
            bindingContext.ModelState.AddModelError("Name", "Name cannot exceed 40 characters.");
            return;
        }

        var role = new AddUpdateRoleDto
        {
            Name = nameValue
        };

        bindingContext.Result = ModelBindingResult.Success(role);
    }
}