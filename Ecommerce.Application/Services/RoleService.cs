using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class RoleService : IRoleServices
{
    private readonly IRoleRepository _roleRepository;


    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public  async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);
        
        return new RoleDto
        {
           Name = role.Name,
           Id = role.Id,
           UserRoles = role.UserRoles
        };


    }

    public async Task<RoleDto> AddRoleAsync(AddUpdateRoleDto roleDto)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = roleDto.Name,
        };
        await _roleRepository.AddRoleAsync(role);

        return new RoleDto
        {
            Name = role.Name,
            Id = role.Id,
            UserRoles = role.UserRoles
        };

    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto)
    {
        var targetRole = await _roleRepository.GetRoleByIdAsync(id);

        targetRole.Name = updateRoleDto.Name;
        // targetRole.UserRoles = updateRoleDto.UserRoles;

        await _roleRepository.UpdateRoleAsync(targetRole);

        return new RoleDto
        {
            Name = targetRole.Name,
            UserRoles = targetRole.UserRoles,
            Id = targetRole.Id,
        };
    }

    public async Task<bool> DeleteRoleByIdAsync(Guid id)
    {
        var targetRole = await _roleRepository.GetRoleByIdAsync(id);
        if (targetRole == null)
        {
            return false;
        }

        await _roleRepository.DeleteRoleByIdAsync(id);
        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllRulesAsync();
        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            UserRoles = role.UserRoles
        });
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesByNameAsync(string name)
    {
        var roleByName = await _roleRepository.GetRolesByName(name);

        return roleByName.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            UserRoles = role.UserRoles,
        });
    }
}