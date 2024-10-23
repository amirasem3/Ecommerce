using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]

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
            var user = await _userServices.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByUsername/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        try
        {
            var user = await _userServices.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userServices.GetUserByEmailAsync(email);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByPhoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
    {
        try
        {
            var user = await _userServices.GetUserByPhoneNumberAsync(phoneNumber);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userServices.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("SearchUserName")]
    public async Task<IActionResult> SearchUserByName([FromQuery] string name)
    {
        try
        {
            var users = await _userServices.GetAllUsersByNameAsync(name);
            return Ok(users);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetUserByRole/{roleName}")]
    public async Task<IActionResult> GetUserByRoleName(string roleName)
    {
        try
        {
            var users = await _userServices.GetUserByRoleAsync(roleName);
            return Ok(users);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
      
    }

    [AllowAnonymous]
    [HttpPost("SignUp")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddUser([FromBody] AddUpdateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var user = await _userServices.AddUserAsync(userDto);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
       
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromQuery] LoginUserDto loginRequestDto)
    {
        try
        {
            var user = await _userServices.AuthenticateUserAsync(loginRequestDto.Username, loginRequestDto.Password);
            return Ok(user);
        }
        catch (Exception e)
        {
           return Unauthorized(e.Message);
        }
    }

    [HttpPut("UpdateUser/{userId}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] AddUpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["User"]!.Errors.Select(e => e.ErrorMessage));
        }
        try
        {
            var user = await _userServices.UpdateUserAsync(userId, updateUserDto);
            return Ok(user);
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }


    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            await _userServices.DeleteUserAsync(userId);
            return Ok($"The user with Id ({userId}) is successfully deleted.");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}