using System.Threading.RateLimiting;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleServices;

    public RoleController(RoleService roleServices)
    {
        _roleServices = roleServices;
    }

    [HttpGet("GetRoleById/{id}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a role by ID.", args: [id]);
        try
        {
            var role = await _roleServices.GetRoleByIdAsync(id);
            LoggerHelper.LogWithDetails($"Role DTO Result", args: [id], retrievedData: role);
            return Ok(role);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong Role ID", args: [id], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        LoggerHelper.LogWithDetails("Attempt to get all the roles.");
        try
        {
            var roles = await _roleServices.GetAllRolesAsync();
            LoggerHelper.LogWithDetails("All Roles", retrievedData: roles);
            return Ok(roles);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors.", retrievedData: e, logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchRoles")]
    public async Task<IActionResult> SearchRoles([FromQuery] string name)
    {
        LoggerHelper.LogWithDetails("Attempt to get a role by name", args: [name]);

        try
        {
            var role = await _roleServices.GetRoleByNameAsync(name);
            LoggerHelper.LogWithDetails("Role DTO Result", args: [name], retrievedData: role);
            return Ok(role);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("AddNewRole")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddRole([FromBody] AddUpdateRoleDto role)
    {
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [role],
                retrievedData: ModelState["Role"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }

        var newRole = await _roleServices.AddRoleAsync(role);
        LoggerHelper.LogWithDetails("New User DTO", args: [role], retrievedData: newRole);
        return Ok(newRole);
    }


    [HttpPut("UpdateRole")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] AddUpdateRoleDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [id, roleDto],
                retrievedData: ModelState["Role"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedRole = await _roleServices.UpdateRoleAsync(id, roleDto);
            LoggerHelper.LogWithDetails("Role Update DTO", args: [id, roleDto], retrievedData: updatedRole);
            return Ok(updatedRole);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", args: [id, roleDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(Guid id)
    {
        
        LoggerHelper.LogWithDetails("Attempt to Delete a role by ID.",args:[id]);
        try
        {
            await _roleServices.DeleteRoleByIdAsync(id);
            LoggerHelper.LogWithDetails($"The role with Id {id} successfully deleted",args:[id]);
            return Ok($"The role with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Wrong ID/Unexpected Errors", args:[id],retrievedData:e);
            return NotFound(e.Message);
        }
    }
}