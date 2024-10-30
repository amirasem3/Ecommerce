using Ecommerce.Application.DTOs.User;
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
public class UserController : ControllerBase
{
    private readonly UserService _userServices;

    public UserController(UserService userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("GetUserById/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            LoggerHelper.LogWithDetails(args: [userId], logLevel: LoggerHelper.LogLevel.Information);
            var user = await _userServices.GetUserByIdAsync(userId);
            LoggerHelper.LogWithDetails("User DTO Result", args: [userId], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User ID.", args: [userId], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByUsername/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        try
        {
            LoggerHelper.LogWithDetails(args: [username], logLevel: LoggerHelper.LogLevel.Information);
            var user = await _userServices.GetUserByUsernameAsync(username);
            LoggerHelper.LogWithDetails("User DTO Result", args: [username], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User Username.", args: [username], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            LoggerHelper.LogWithDetails(args: [email], logLevel: LoggerHelper.LogLevel.Information);
            var user = await _userServices.GetUserByEmailAsync(email);
            LoggerHelper.LogWithDetails("User DTO Result", args: [email], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User Email.", args: [email], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByPhoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
    {
        try
        {
            LoggerHelper.LogWithDetails(args: [phoneNumber], logLevel: LoggerHelper.LogLevel.Information);
            var user = await _userServices.GetUserByPhoneNumberAsync(phoneNumber);
            LoggerHelper.LogWithDetails("User DTO Result", args: [phoneNumber], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User Phone Number.", args: [phoneNumber], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        LoggerHelper.LogWithDetails("Attempt to get all users.");
        try
        {
            var users = await _userServices.GetAllUsersAsync();
            LoggerHelper.LogWithDetails("All Users", retrievedData: users);
            return Ok(users);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", retrievedData: e);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchUserName")]
    public async Task<IActionResult> SearchUserByName([FromQuery] string name)
    {
        LoggerHelper.LogWithDetails("Attempt to search users by name", args: [name]);
        try
        {
            var users = await _userServices.GetAllUsersByNameAsync(name);
            LoggerHelper.LogWithDetails($"All users which have this name in their firstnames found successfully.",
                retrievedData: users
                , args: [name]);
            return Ok(users);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", args: [name], retrievedData: e);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByRole/{roleName}")]
    public async Task<IActionResult> GetUserByRoleName(string roleName)
    {
        LoggerHelper.LogWithDetails("Attempt to search users by role name", args: [roleName]);
        try
        {
            var users = await _userServices.GetUserByRoleAsync(roleName);
            LoggerHelper.LogWithDetails($"All users which have role named {roleName} found successfully.",
                args: [roleName], retrievedData: users);
            return Ok(users);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong Role Name.", args: [roleName], retrievedData: e);
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("SignUp")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddUser([FromBody] AddUpdateUserDto userDto)
    {
        LoggerHelper.LogWithDetails("Attempt to SignUp an user.", args: [userDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [userDto],
                retrievedData: ModelState["User"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var user = await _userServices.AddUserAsync(userDto);
            LoggerHelper.LogWithDetails("User DTO Result", args: [userDto], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User Data.", args: [userDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromQuery] LoginUserDto loginRequestDto)
    {
        LoggerHelper.LogWithDetails("Attempts to Login.", args: [loginRequestDto]);
        try
        {
            var user = await _userServices.AuthenticateUserAsync(loginRequestDto.Username, loginRequestDto.Password);
            LoggerHelper.LogWithDetails("Successful Login.", args: [loginRequestDto], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Username or Password!", args: [loginRequestDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return Unauthorized(e.Message);
        }
    }

    [HttpPut("UpdateUser/{userId}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] AddUpdateUserDto updateUserDto)
    {
        LoggerHelper.LogWithDetails("Attempt to Update an user.", args: [updateUserDto]);

        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [updateUserDto],
                retrievedData: ModelState["User"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var user = await _userServices.UpdateUserAsync(userId, updateUserDto);
            LoggerHelper.LogWithDetails("User DTO Result", args: [userId, updateUserDto], retrievedData: user);
            return Ok(user);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User Data.", args: [userId, updateUserDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e);
        }
    }


    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        LoggerHelper.LogWithDetails("Attempt to Delete an User.", args: [userId]);
        try
        {
            await _userServices.DeleteUserAsync(userId);
            LoggerHelper.LogWithDetails($"The user with ID {userId} is Successfully Deleted.", args: [userId]);
            return Ok($"The user with Id ({userId}) is successfully deleted.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong User ID.", args: [userId], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}