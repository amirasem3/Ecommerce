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
    private readonly ILogger<RoleController> _logger;

    public RoleController(RoleService roleServices,ILogger<RoleController> logger)
    {
        _roleServices = roleServices;
        _logger = logger;
    }

    [HttpGet("GetRoleById/{id}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            var role = await _roleServices.GetRoleByIdAsync(id);
            return Ok(role);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Wrong Role ID", args: [id], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        LoggerHelper.LogWithDetails(_logger);
        try
        {
            var roles = await _roleServices.GetAllRolesAsync();
            return Ok(roles);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors.", retrievedData: e, logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchRoles")]
    public async Task<IActionResult> SearchRoles([FromQuery] string name)
    {
        LoggerHelper.LogWithDetails(_logger,args: [name]);

        try
        {
            var role = await _roleServices.GetRoleByNameAsync(name);
            return Ok(role);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [name], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("AddNewRole")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddRole([FromBody] AddUpdateRoleDto newRoleDto)
    {
        LoggerHelper.LogWithDetails(_logger,args:[newRoleDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [newRoleDto],
                retrievedData: ModelState["Role"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }

        var newRole = await _roleServices.AddRoleAsync(newRoleDto);
        return Ok(newRole);
    }


    [HttpPut("UpdateRole")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] AddUpdateRoleDto updateRoleDto)
    {
        LoggerHelper.LogWithDetails(_logger,args:[id,updateRoleDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [id, updateRoleDto],
                retrievedData: ModelState["Role"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedRole = await _roleServices.UpdateRoleAsync(id, updateRoleDto);
            return Ok(updatedRole);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [id, updateRoleDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            await _roleServices.DeleteRoleByIdAsync(id);
            return Ok($"The role with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Wrong ID/Unexpected Errors", args: [id], retrievedData: e);
            return NotFound(e.Message);
        }
    }
}