using Ecommerce.Application.DTOs;
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
        try
        {
            var role = await _roleServices.GetRoleByIdAsync(id);
                return Ok(role);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleServices.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("SearchRoles")]
    public async Task<IActionResult> SearchRoles([FromQuery] String name)
    {
        try
        {
            var roles = await _roleServices.GetRoleByNameAsync(name);
            return Ok(roles);
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
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }
        var user = await _roleServices.AddRoleAsync(role);
        return Ok(user);
    }


    [HttpPut("UpdateRole")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] AddUpdateRoleDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Role"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedRole = await _roleServices.UpdateRoleAsync(id, roleDto);
            return Ok(updatedRole);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(Guid id)
    {
        try
        {
            await _roleServices.DeleteRoleByIdAsync(id);
            return Ok($"The role with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}