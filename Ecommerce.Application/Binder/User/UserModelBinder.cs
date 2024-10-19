using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        var firstNameValue = bindingContext.ValueProvider.GetValue("firstName").FirstValue;
        var lastNameValue = bindingContext.ValueProvider.GetValue("lastName").FirstValue;
        var usernameValue = bindingContext.ValueProvider.GetValue("username").FirstValue;
        var emailValue = bindingContext.ValueProvider.GetValue("email").FirstValue;
        var phoneNumberValue = bindingContext.ValueProvider.GetValue("phoneNumber").FirstValue;
        var passwordValue = bindingContext.ValueProvider.GetValue("password").FirstValue;
        var roleNameValue = bindingContext.ValueProvider.GetValue("roleName").FirstValue;


        if (bindingContext.ModelMetadata.ModelType == typeof(UpdateUserDto))
        {
            var idValue = bindingContext.ValueProvider.GetValue("id").FirstValue;
            if (string.IsNullOrWhiteSpace(idValue))
            {
                bindingContext.ModelState.AddModelError("id", "You cannot update without valid user ID.");
                return;
            }
        }

        if (bindingContext.ModelMetadata.ModelType == typeof(LoginUserDto))
        {
            var authUser = await _userService.AuthenticateUserAsync(usernameValue, passwordValue);
            if (authUser == null)
            {
                bindingContext.ModelState.AddModelError("auth", "Incorrect Username or Password!");
                return;
            }
        }

        if (bindingContext.ModelMetadata.ModelType == typeof(RegisterUserDto) || bindingContext.ModelMetadata.ModelType == typeof(UpdateUserDto))
        {
              if (string.IsNullOrWhiteSpace(firstNameValue))
        {
            // Add error to ModelState
            bindingContext.ModelState.AddModelError("firstName", "Firstname is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(lastNameValue))
        {
            bindingContext.ModelState.AddModelError("lastName", "Lastname is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(usernameValue))
        {
            bindingContext.ModelState.AddModelError("username", "Username is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(emailValue))
        {
            bindingContext.ModelState.AddModelError("email", "Email is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(phoneNumberValue))
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "PhoneNumber is required.");
            return;
        }

        

        if (string.IsNullOrWhiteSpace(roleNameValue))
        {
            bindingContext.ModelState.AddModelError("roleName", "Role name is required.");
            return;
        }

        var roleExistEmail = await _userService.GetUserByEmailAsync(emailValue);
        var roleExistUsername = await _userService.GetUserByUsernameAsync(usernameValue);
        var roleExistPhoneNumber = await _userService.GetUserByPhoneNumberAsync(phoneNumberValue);
        if (roleExistEmail != null)
        {
            bindingContext.ModelState.AddModelError("email", "This Email is already exists.");
        }

        if (roleExistUsername != null)
        {
            bindingContext.ModelState.AddModelError("username", "This Username already exists.");
        }

        if (roleExistPhoneNumber != null)
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "This PhoneNumber is already exists.");
        }

        //FirstName
        if (firstNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("firstName", "Firstname cannot exceed 40 characters.");
            return;
        }

        if (!Regex.Match(firstNameValue,@"^[a-zA-Z''آ-ی\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("firstName", "Numbers in FirstName is not allowed.");
            return;
        }

        //LastName
        if (lastNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("lastName", "Lastname cannot exceed 40 characters.");
            return;
        }

        if (!Regex.Match(lastNameValue,@"^[a-zA-Z''آ-ی\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("lastName", "Numbers in Lastname is not allowed.");
            return;
        }
        //Email
        if (emailValue.Length > 30)
        {
            bindingContext.ModelState.AddModelError("email", "Email cannot exceed 30 characters.");
            return;
        }

        if (!emailValue.Contains("@"))
        {
            bindingContext.ModelState.AddModelError("email", "Enter valid email address.");
            return;
        }
        
        //PhoneNumber
        if (phoneNumberValue.Length > 20)
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "PhoneNumber cannot exceed 20 characters");
            return;
        }

        if (!Regex.Match(phoneNumberValue,@"^[0-9]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "Non-numeric characters are not allowed in PhoneNumber.");
            return;
        }
        //Username
        if (usernameValue.Length > 30)
        {
            bindingContext.ModelState.AddModelError("username", "Username cannot exceed 30 characters.");
            return;
        }

        if (!Regex.Match(usernameValue,@"^[a-zA-Z0-9''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("username", "Invalid characters in Username.");
            return;
        }
        //Password
        if (string.IsNullOrWhiteSpace(passwordValue))
        {
            bindingContext.ModelState.AddModelError("password", "Password is required.");
            return;
        }
        }
      

        //Username
        if (usernameValue.Length > 30)
        {
            bindingContext.ModelState.AddModelError("username", "Username cannot exceed 30 characters.");
            return;
        }

        if (!Regex.Match(usernameValue,@"^[a-zA-Z0-9''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("username", "Invalid characters in Username.");
            return;
        }
        //Password
        if (string.IsNullOrWhiteSpace(passwordValue))
        {
            bindingContext.ModelState.AddModelError("password", "Password is required.");
            return;
        }

      
        

        var user = new RegisterUserDto()
        {
            FirstName = firstNameValue,
            LastName = lastNameValue,
            Email = emailValue,
            Password = passwordValue,
            PhoneNumber = phoneNumberValue,
            RoleName = roleNameValue,
            Username = usernameValue
        };

        
        if (bindingContext.ModelMetadata.ModelType == typeof(UpdateUserDto))
        {
            var updatedUser = new UpdateUserDto()
            {
                Id = bindingContext.ValueProvider.GetValue("id").FirstValue,
                FirstName = firstNameValue,
                LastName = lastNameValue,
                Email = emailValue,
                PhoneNumber = phoneNumberValue,
                Password = passwordValue,
                RoleName = roleNameValue,
                Username = usernameValue
            };
            bindingContext.Result = ModelBindingResult.Success(updatedUser);
            return;
        }

        if (bindingContext.ModelMetadata.ModelType == typeof(LoginUserDto))
        {
            var loginUser = new LoginUserDto()
            {
                Username = usernameValue,
                Password = passwordValue
            };
            bindingContext.Result = ModelBindingResult.Success(loginUser);
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(user);
    }
}