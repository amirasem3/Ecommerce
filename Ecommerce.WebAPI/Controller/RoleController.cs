using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;


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
        if (role == null)
        {
            return NotFound($"The Product With Id {id} does not exist!");
        }

        return Ok(role);
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
        var roles = await _roleServices.GetAllRolesByNameAsync(name);
        return Ok(roles);
    }
    [HttpPost("AddRole")]
    public async Task<IActionResult> AddRole([FromBody] AddUpdateRoleDto updateRoleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createdRole =  await _roleServices.AddRoleAsync(updateRoleDto);
        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
    }
    [HttpPut("UpdateRole")]
    public async Task<IActionResult> UpdateRole([FromQuery]Guid id, [FromQuery] AddUpdateRoleDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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