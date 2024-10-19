using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
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
    private readonly IRoleServices _roleServices;

    public RoleController(IRoleServices roleServices)
    {
        _roleServices = roleServices;
    }

    [HttpGet("GetRoleById/{id}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var role = await _roleServices.GetRoleByIdAsync(id);

        if (role != null)
        {
            return Ok(role);
        }

        return NotFound($"There is no role with Id {id}.");
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
        var roles = await _roleServices.GetRoleByNameAsync(name);
        return Ok(roles);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddRole([ModelBinder(typeof(RoleModelBinder))] AddUpdateRoleDto role)
    {
        if (!ModelState.IsValid)
        {
            if (ModelState.ContainsKey("Name"))
            {
                var nameErrors = ModelState["Name"].Errors;
                if (nameErrors.Count > 0)
                {
                    return BadRequest(new { message = nameErrors[0].ErrorMessage });
                }
            }
            return BadRequest(new { message = "Invalid input" });
        }
        var user = await _roleServices.AddRoleAsync(role);
       

        return Ok(user);
    }


    [HttpPut("UpdateRole")]
    public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] AddUpdateRoleDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updatedRole = await _roleServices.UpdateRoleAsync(id, roleDto);
        return Ok(updatedRole);
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(Guid id)
    {
        var deleted = await _roleServices.DeleteRoleByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok($"The role with Id {id} successfully deleted");
    }
}