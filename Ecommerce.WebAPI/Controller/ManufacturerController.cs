using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;
[ApiController]
[Route("api/[controller]")]
public class ManufacturerController : ControllerBase
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }
    [HttpGet("GetManufacturerById/{manufacturerId}")]
    public async Task<IActionResult> GetManufacturerById(Guid manufacturerId)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);
        var manufacturer2 = await _manufacturerService.GetManufactureProductAsync(manufacturerId);
        var result = new
        {
            manufacturer.Id,
            manufacturer.Name,
            manufacturer.OwnerName,
            manufacturer.Email,
            manufacturer.PhoneNumber,
            manufacturer.Address,
            manufacturer.Rate,
            manufacturer.Status,
        };
        if (manufacturer != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no user with Id {manufacturerId}.");
    }

    // [HttpGet("GetUserByUsername/{username}")]
    // public async Task<IActionResult> GetUserByUsername(string username)
    // {
    //     var user = await _manufacturerService.GetUserByUsernameAsync(username);
    //     var user2 = await _manufacturerService.GetUserRoleAsync(user.Id);
    //     var result = new
    //     {
    //         user.Id,
    //         user.FirstName,
    //         user.LastName,
    //         user.Email,
    //         user.PhoneNumber,
    //         user.Username,
    //         Roles = user2.UserRoles.Select(ur => new
    //         {
    //             ur.Role.Id,
    //             ur.Role.Name
    //         })
    //     };
    //     if (user!=null)
    //     {
    //         return Ok(result);
    //     }
    //
    //     return NotFound($"There is no user with username {username}");
    // }
    //
    // [HttpGet("GetUserByEmail/{email}")]
    // public async Task<IActionResult> GetUserByEmail(string email)
    // {
    //     var user = await _manufacturerService.GetUserByEmailAsync(email);
    //     var user2 = await _manufacturerService.GetUserRoleAsync(user.Id);
    //     var result = new
    //     {
    //         user.Id,
    //         user.FirstName,
    //         user.LastName,
    //         user.Email,
    //         user.PhoneNumber,
    //         user.Username,
    //         Roles = user2.UserRoles.Select(ur => new
    //         {
    //             ur.Role.Id,
    //             ur.Role.Name
    //         })
    //     };
    //     if (user != null)
    //     {
    //         return Ok(result);
    //     }
    //
    //     return NotFound($"There is no user with Email {email}");
    // }
    //
    // [HttpGet("GetUserByPhoneNumber/{phoneNumber}")]
    // public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
    // {
    //     var user = await _manufacturerService.GetUserByPhoneNumberAsync(phoneNumber);
    //     var user2 = await _manufacturerService.GetUserRoleAsync(user.Id);
    //     var result = new
    //     {
    //         user.Id,
    //         user.FirstName,
    //         user.LastName,
    //         user.Email,
    //         user.PhoneNumber,
    //         user.Username,
    //         Roles = user2.UserRoles.Select(ur => new
    //         {
    //             ur.Role.Id,
    //             ur.Role.Name
    //         })
    //     };
    //     if (user != null)
    //     {
    //         return Ok(result);
    //         
    //     }
    //
    //     return NotFound($"There is no user with phone number {phoneNumber}");
    // }
    //
    // [HttpGet("GetAllUsers")]
    // public async Task<IActionResult> GetAllUsers()
    // {
    //     var users = await _manufacturerService.GetAllUsersAsync();
    //     var result = new List<object>(); 
    //     foreach (var user in users)
    //     {
    //         var roles = new List<object>();
    //       var  user2 = await _manufacturerService.GetUserRoleAsync(user.Id);
    //
    //         foreach (var userRole in user2.UserRoles)
    //         {
    //             roles.Add(new
    //             {
    //                 userRole.Role.Id,
    //                 userRole.Role.Name
    //             });
    //         }
    //
    //         result.Add(new
    //         {
    //             user.Id,
    //             user.FirstName,
    //             user.LastName,
    //             user.Username,
    //             user.Email,
    //             user.PhoneNumber,
    //             Roles = roles 
    //         });
    //     }
    //     
    //     
    //
    //     return Ok(result);
    // }
    //
    // [HttpGet("SearchUserName")]
    // public async Task<IActionResult> SearchUserByName([FromQuery]string name)
    // {
    //     var users = await _manufacturerService.GetAllUsersByNameAsync(name);
    //     var result = new List<object>(); 
    //     foreach (var user in users)
    //     {
    //         var roles = new List<object>();
    //         var  user2 = await _manufacturerService.GetUserRoleAsync(user.Id);
    //
    //         foreach (var userRole in user2.UserRoles)
    //         {
    //             roles.Add(new
    //             {
    //                 userRole.Role.Id,
    //                 userRole.Role.Name
    //             });
    //         }
    //
    //         result.Add(new
    //         {
    //             user.Id,
    //             user.FirstName,
    //             user.LastName,
    //             user.Username,
    //             user.Email,
    //             user.PhoneNumber,
    //             Roles = roles 
    //         });
    //     }
    //
    //     return Ok(result);
    //
    // }
    //
    // [HttpGet("GetUserRole")]
    // public async Task<IActionResult> GetUserRole([FromQuery] Guid manufacturerId)
    // {
    //     var user = await _manufacturerService.GetUserRoleAsync(manufacturerId);
    //     if (user!= null)
    //     {
    //         var result = new
    //         {
    //             user.Id,
    //             user.Username,
    //             Roles = user.UserRoles.Select(ur => new
    //             {
    //                 ur.Role.Id,
    //                 ur.Role.Name
    //             })
    //         };
    //         return Ok(result);
    //     }
    //
    //     return NotFound($"There is no rule for user with ID {manufacturerId}");
    // }
    //
    //
    // [HttpPost("SignUp")]
    // public async Task<IActionResult> AddUser([FromBody] RegisterUserDto userDto)
    // {
    //     var user = await _manufacturerService.AddUserAsync(userDto);
    //
    //     return Ok(user);
    // }
    //
    // [HttpPost("AssignUserRole")]
    // public async Task<IActionResult> AssignRoleToUser([FromQuery]Guid manufacturerId, [FromQuery]Guid roleId)
    // {
    //     await _manufacturerService.AssignRoleToUserAsync(manufacturerId, roleId);
    //     return Ok(_manufacturerService.GetManufacturerByIdAsync(manufacturerId));
    // }
    //
    // [HttpPost("Login")]
    // public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginRequestDto)
    // {
    //     var user = await _manufacturerService.AuthenticateUserAsync(loginRequestDto.Username, loginRequestDto.Password);
    //     if (user == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return Ok(user);
    //         
    // }
    //
    // [HttpPut("UpdateUser")]
    // public async Task<IActionResult> UpdateUser([FromQuery] Guid manufacturerId, [FromQuery] UpdateUserDto updateUserDto)
    // {
    //     var targetUser = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);
    //     if (targetUser!=null)
    //     {
    //         var user = await _manufacturerService.UpdateProductAsync(manufacturerId, updateUserDto);
    //         return Ok(user);
    //     }
    //
    //     return NotFound($"There is no user with ID {manufacturerId}");
    //
    // }
    //
    //
    //
    // [HttpDelete("DeleteUser/{manufacturerId}")]
    // public async Task<IActionResult> DeleteUser(Guid manufacturerId)
    // {
    //     var deleted = await _manufacturerService.DeleteUserAsync(manufacturerId);
    //     if (deleted)
    //     {
    //         return Ok($"The user with Id ({manufacturerId}) is successfully deleted.");
    //     }
    //
    //     return NotFound($"There is no user with ID {manufacturerId}.");
    // } 
}