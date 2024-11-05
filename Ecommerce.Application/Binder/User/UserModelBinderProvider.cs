using System;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.User;

public class UserModelBinderProvider : IModelBinderProvider
{

    private readonly UserService _userService;

    public UserModelBinderProvider(UserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateUserDto))
        {
            return new UserModelBinder(_userService);
        }

        return null;
    }
}