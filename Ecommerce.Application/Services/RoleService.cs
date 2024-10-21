using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class RoleService : IRoleServices
{
    private readonly IRoleRepository _roleRepository;
    public const string RoleException = "Role Not Found!";


    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public  async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);
        if (role == null)
        {
            throw new Exception(RoleException);
        }
        
        return new RoleDto
        {
           Name = role.Name,
           Id = role.Id,
        };


    }

    public async Task<RoleDto> AddRoleAsync(AddUpdateRoleDto newRole)
    {
       
        var role = new Role
        {
            Name = newRole.Name,
            Id = Guid.NewGuid()
        };
        await _roleRepository.AddRoleAsync(role);

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };

    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto)
    {
        var targetRole = await _roleRepository.GetRoleByIdAsync(id);

        if (targetRole == null)
        {
            throw new Exception(RoleException);
        }

        targetRole.Name = updateRoleDto.Name;
        // targetRole.UserRoles = updateRoleDto.UserRoles;

        await _roleRepository.UpdateRoleAsync(targetRole);

        return new RoleDto
        {
            Name = targetRole.Name,
            Id = targetRole.Id,
        };
    }

    public async Task<bool> DeleteRoleByIdAsync(Guid id)
    {
        var targetRole = await _roleRepository.GetRoleByIdAsync(id);
        if (targetRole == null)
        {
            throw new Exception(RoleException);
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
        });
    }

    public async Task<RoleDto> GetRoleByNameAsync(string name)
    {
        var role = await _roleRepository.GetRoleByName(name);
        
        if (role == null)
        {
            throw new Exception(RoleException);
        }

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };
    }
}