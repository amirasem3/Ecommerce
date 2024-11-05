using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce.Application.Binder.User;

public class UserModelBinder : IModelBinder
{
    private readonly UserService _userService;

    public UserModelBinder(UserService userService)
    {
        _userService = userService;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;


        AddUpdateUserDto? userModel;
        try
        {
            userModel = JsonSerializer.Deserialize<AddUpdateUserDto>(requestBody, new JsonSerializerOptions
            { 
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("User", "Invalid JSON format.");
            return;
        }

        if (userModel == null)
        {
            bindingContext.ModelState.AddModelError("User", "Invalid user data.");
            return;
        }

        try
        {
            var roleExistEmail = await _userService.GetUserByEmailAsync(userModel.Email);
            if (roleExistEmail != null)
            {
                bindingContext.ModelState.AddModelError("User", "This Email is already exists.");
            }
        }
        catch (Exception e)
        {
            if (e.Message != UserService.UserException)
            {
                bindingContext.ModelState.AddModelError("User",
                    "An unexpected error occurred while checking the email uniqueness.");
            }
        }
        
        try
        {
            var roleExistUsername = await _userService.GetUserByUsernameAsync(userModel.Username);
            if (roleExistUsername != null)
            {
                bindingContext.ModelState.AddModelError("User", "This Username already exists.");
            }
        }
        catch (Exception e)
        {
            if (e.Message != UserService.UserException)
            {
                bindingContext.ModelState.AddModelError("User",
                    "An unexpected error occurred while checking the username uniqueness.");
            }
        }
        
        try
        {
            var roleExistPhoneNumber = await _userService.GetUserByPhoneNumberAsync(userModel.PhoneNumber);
            if (roleExistPhoneNumber != null)
            {
                bindingContext.ModelState.AddModelError("User", "This PhoneNumber is already exists.");
            }
        }
        catch (Exception e)
        {
            if (e.Message != UserService.UserException)
            {
                bindingContext.ModelState.AddModelError("User",
                    "An unexpected error occurred while checking the phone number uniqueness.");
            }
        }

        //FirstName
        if (string.IsNullOrWhiteSpace(userModel.FirstName))
        {
            bindingContext.ModelState.AddModelError("User", "Firstname is required.");
        }

        if (userModel.FirstName.Length > 40)
        {
            bindingContext.ModelState.AddModelError("User", "Firstname cannot exceed 40 characters.");
        }

        if (!Regex.Match(userModel.FirstName, @"^[a-zA-Z''آ-ی\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("User", "Numbers in FirstName is not allowed.");
        }

        //LastName
        if (string.IsNullOrWhiteSpace(userModel.LastName))
        {
            bindingContext.ModelState.AddModelError("User", "Lastname is required.");
        }

        if (userModel.LastName.Length > 40)
        {
            bindingContext.ModelState.AddModelError("User", "Lastname cannot exceed 40 characters.");
        }

        if (!Regex.Match(userModel.LastName, @"^[a-zA-Z''آ-ی\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("User", "Numbers in Lastname is not allowed.");
        }

        //Email
        if (string.IsNullOrWhiteSpace(userModel.Email))
        {
            bindingContext.ModelState.AddModelError("User", "Email is required.");
        }

        if (userModel.Email.Length > 30)
        {
            bindingContext.ModelState.AddModelError("User", "Email cannot exceed 30 characters.");
        }

        if (!userModel.Email.Contains("@"))
        {
            bindingContext.ModelState.AddModelError("User", "Enter valid email address.");
        }

        //PhoneNumber
        if (string.IsNullOrWhiteSpace(userModel.PhoneNumber))
        {
            bindingContext.ModelState.AddModelError("User", "PhoneNumber is required.");
        }

        if (userModel.PhoneNumber.Length > 20)
        {
            bindingContext.ModelState.AddModelError("User", "PhoneNumber cannot exceed 20 characters");
        }

        if (!Regex.Match(userModel.PhoneNumber, @"^[0-9]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("User", "Non-numeric characters are not allowed in PhoneNumber.");
        }

        //Username
        if (string.IsNullOrWhiteSpace(userModel.Username))
        {
            bindingContext.ModelState.AddModelError("User", "Username is required.");
        }

        if (userModel.Username.Length > 30)
        {
            bindingContext.ModelState.AddModelError("User", "Username cannot exceed 30 characters.");
        }

        if (!Regex.Match(userModel.Username, @"^[a-zA-Z0-9''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("User", "Invalid characters in Username.");
        }

        //Password
        if (string.IsNullOrWhiteSpace(userModel.Password))
        {
            bindingContext.ModelState.AddModelError("User", "Password is required.");
        }

        //Role Name
        if (string.IsNullOrWhiteSpace(userModel.RoleName))
        {
            bindingContext.ModelState.AddModelError("User", "Role name is required.");
        }


        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;

        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: userModel);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(userModel);
    }
}