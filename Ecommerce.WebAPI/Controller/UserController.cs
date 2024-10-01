using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("GetUserById/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _userServices.GetUserByIdAsync(userId);
        // var user2 = await _userServices.GetUserRoleAsync(userId);
        var result = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Username,
            user.RoleName,
            user.RoleId
        };
        if (user != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no user with Id {userId}.");
    }

    [HttpGet("GetUserByUsername/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userServices.GetUserByUsernameAsync(username);
        // var user2 = await _userServices.GetUserRoleAsync(user.RoleId);
        var result = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Username,
            user.RoleName,
            user.RoleId
        };
        if (user!=null)
        {
            return Ok(result);
        }

        return NotFound($"There is no user with username {username}");
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userServices.GetUserByEmailAsync(email);
        // var user2 = await _userServices.GetUserRoleAsync(user.Id);
        var result = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Username,
         user.RoleName,
         user.RoleId,
        };
        if (user != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no user with Email {email}");
    }
    
    [HttpGet("GetUserByPhoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
    {
        var user = await _userServices.GetUserByPhoneNumberAsync(phoneNumber);
        // var user2 = await _userServices.GetUserRoleAsync(user.Id);
        var result = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Username,
           user.RoleId,
           user.RoleName
        };
        if (user != null)
        {
            return Ok(result);
            
        }

        return NotFound($"There is no user with phone number {phoneNumber}");
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userServices.GetAllUsersAsync();
        var result = new List<object>();
        if (users == null)
        {
            return NotFound();
        }
        foreach (var user in users)
        {
            var roles = new List<object>();
          // var  user2 = await _userServices.GetUserRoleAsync(user.Id);

          

            result.Add(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.PhoneNumber,
                user.RoleId,
                user.RoleName
            });
        }
        
        

        return Ok(result);
    }

    [HttpGet("SearchUserName")]
    public async Task<IActionResult> SearchUserByName([FromQuery]string name)
    {
        var users = await _userServices.GetAllUsersByNameAsync(name);
        var result = new List<object>(); 
        foreach (var user in users)
        {
            var roles = new List<object>();
            // var  user2 = await _userServices.GetUserRoleAsync(user.Id);
            

            result.Add(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.PhoneNumber,
                user.RoleId,
                user.RoleName
            });
        }

        return Ok(result);

    }

    [HttpGet("GetUserRole")]
    public async Task<IActionResult> GetUserRole([FromQuery] Guid userId)
    {
        var user = await _userServices.GetUserByIdAsync(userId);
        if (user!= null)
        {
            var result = new
            {
                user.Id,
                user.Username,
              user.RoleId,
              user.RoleName
            };
            return Ok(result);
        }

        return NotFound($"There is no rule for user with ID {userId}");
    }
    

    [HttpPost("SignUp")]
    public async Task<IActionResult> AddUser([FromBody] RegisterUserDto userDto)
    {
        var user = await _userServices.AddUserAsync(userDto);

        return Ok(user);
    }
    
    [HttpPost("AssignUserRole")]
    public async Task<IActionResult> AssignRoleToUser([FromQuery]Guid userId, [FromQuery]Guid roleId)
    {
        await _userServices.AssignRoleToUserAsync(userId, roleId);
        return Ok(_userServices.GetUserByIdAsync(userId));
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginRequestDto)
    {
        var user = await _userServices.AuthenticateUserAsync(loginRequestDto.Username, loginRequestDto.Password);
        if (user == null)
        {
            return NotFound();
        }
    
        return Ok(user);
            
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromQuery] Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        var targetUser = await _userServices.GetUserByIdAsync(userId);
        if (targetUser!=null)
        {
            var user = await _userServices.UpdateUserAsync(userId, updateUserDto);
            return Ok(user);
        }

        return NotFound($"There is no user with ID {userId}");

    }

   

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var deleted = await _userServices.DeleteUserAsync(userId);
        if (deleted)
        {
            return Ok($"The user with Id ({userId}) is successfully deleted.");
        }

        return NotFound($"There is no user with ID {userId}.");
    } 
    
}