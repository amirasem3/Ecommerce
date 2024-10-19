using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder;

public class RoleModelBinderProvider : IModelBinderProvider
{
    private readonly RoleService _roleService;

    public RoleModelBinderProvider(RoleService roleService)
    {
        _roleService = roleService;
    }
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateRoleDto))
        {
            return new RoleModelBinder(_roleService);
        }

        return null;
    }
}