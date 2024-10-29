using Ecommerce.Application.DTOs.User;
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
public class UserController : ControllerBase
{
    private readonly UserService _userServices;
    private readonly ILogger<UserController> _logger;
    private const string UserControllerString = "User Controller";

    public UserController(UserService userServices, ILogger<UserController> logger)
    {
        _userServices = userServices;
        _logger = logger;
    }

    [HttpGet("GetUserById/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetUserById ({Id})", UserControllerString, userId);
            _logger.LogInformation("{UserController} Call {UserService}'s GetUserByIdAsync.", UserControllerString,
                UserService.Location);
            var user = await _userServices.GetUserByIdAsync(userId);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetUserById", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByUsername/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetUserByUsername ({Username})", UserControllerString,
                username);
            _logger.LogInformation("{UserController} Call {UserService}'s GetUserByUsernameAsync.",
                UserControllerString, UserService.Location);
            var user = await _userServices.GetUserByUsernameAsync(username);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetUserByUsername", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetUserByEmail ({Email})", UserControllerString, email);
            _logger.LogInformation("{UserController} Call {UserService}'s GetUserByEmailAsync.", UserControllerString,
                UserService.Location);
            var user = await _userServices.GetUserByEmailAsync(email);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetUserByEmail", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByPhoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetUserByPhoneNumber ({PhoneNumber})", UserControllerString,
                phoneNumber);
            _logger.LogInformation("{UserController} Call {UserService}'s GetUserByPhoneNumberAsync.",
                UserControllerString, UserService.Location);
            var user = await _userServices.GetUserByPhoneNumberAsync(phoneNumber);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetUserByPhoneNumber", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetAllUsers)", UserControllerString);
            _logger.LogInformation("{UserController} Call {UserService}'s GetAllUsersAsync.", UserControllerString,
                UserService.Location);
            var users = await _userServices.GetAllUsersAsync();
            _logger.LogInformation("All user retrieved successfully.");
            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetAllUsers", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchUserName")]
    public async Task<IActionResult> SearchUserByName([FromQuery] string name)
    {
        try
        {
            _logger.LogInformation("({UserController}Call SearchUserByName({Name})", UserControllerString, name);
            _logger.LogInformation("{UserController} Call {UserService}'s GetAllUsersByNameAsync.",
                UserControllerString, UserService.Location);
            var users = await _userServices.GetAllUsersByNameAsync(name);
            _logger.LogInformation("All users,have {Name} in their firstnames., found successfully", name);
            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "SearchUserByName", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByRole/{roleName}")]
    public async Task<IActionResult> GetUserByRoleName(string roleName)
    {
        try
        {
            _logger.LogInformation("({UserController}Call GetUserByRoleName({RoleName})", UserControllerString,
                roleName);
            _logger.LogInformation("{UserController} Call {UserService}'s GetUserByRoleAsync.", UserControllerString,
                UserService.Location);
            var users = await _userServices.GetUserByRoleAsync(roleName);
            _logger.LogInformation("All users,have {RoleName} in their RoleNames., found successfully", roleName);
            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "GetUserByRoleName", e.Message);
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("SignUp")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddUser([FromBody] AddUpdateUserDto userDto)
    {
        _logger.LogInformation("({UserController}Call AddUser({NewUserInfo})", UserControllerString,
            userDto.ToString());
        if (!ModelState.IsValid)
        {
            var errors = string.Join("\n, ", ModelState["User"]!.Errors.Select(e => e.ErrorMessage));

            _logger.LogError("{UserController}:{FunctionName}: Binding Error: {BindingError}", UserControllerString,
                "AddUser", errors);

            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            _logger.LogInformation("{UserController} Call {UserService}'s AddUserAsync.", UserControllerString,
                UserService.Location);
            var user = await _userServices.AddUserAsync(userDto);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "AddUser", e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromQuery] LoginUserDto loginRequestDto)
    {
        _logger.LogInformation("({UserController} : Login({NewUserInfo})", UserControllerString,
            loginRequestDto.ToString());
        try
        {
            _logger.LogInformation("{UserController} Call {UserService}'s AuthenticateUserAsync.", UserControllerString,
                UserService.Location);
            var user = await _userServices.AuthenticateUserAsync(loginRequestDto.Username, loginRequestDto.Password);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "Login", e.Message);
            return Unauthorized(e.Message);
        }
    }

    [HttpPut("UpdateUser/{userId}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] AddUpdateUserDto updateUserDto)
    {
        
        _logger.LogInformation("({UserController} : Update({UpdateUserInfo})", UserControllerString,
            updateUserDto.ToString());
        if (!ModelState.IsValid)
        {
            
            var errors = string.Join("\n, ", ModelState["User"]!.Errors.Select(e => e.ErrorMessage));

            _logger.LogError("{UserController}:{FunctionName}: Binding Error: {BindingError}", UserControllerString,
                "UpdateUser", errors);
            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            _logger.LogInformation("{UserController} Call {UserService}'s UpdateUserAsync.", UserControllerString,
                UserService.Location);
            var user = await _userServices.UpdateUserAsync(userId, updateUserDto);
            _logger.LogInformation("{UserController} {FoundUser} {User}", UserControllerString, UserService.FoundUser,
                user.ToString());
            return Ok(user);
        }
        catch (Exception e)
        {
            
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "UpdateUser", e.Message);
            return NotFound(e);
        }
    }


    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        _logger.LogInformation("({UserController} : DeleteUser({UserId})", UserControllerString,
            userId);
        try
        {
            _logger.LogInformation("{UserController} Call {UserService}'s DeleteUserAsync.", UserControllerString,
                UserService.Location);
            await _userServices.DeleteUserAsync(userId);
            _logger.LogInformation("The user with Id ({UserId}) is successfully deleted.", userId);
            return Ok($"The user with Id ({userId}) is successfully deleted.");
        }
        catch (Exception e)
        {
            _logger.LogError("{UserController}: {FunctionName} :  Exception : {ExceptionMessage}", UserControllerString,
                "DeleteUser", e.Message);
            return NotFound(e.Message);
        }
    }
}