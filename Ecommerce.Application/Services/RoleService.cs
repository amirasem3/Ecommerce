using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> GetRoleByIdAsync(Guid id)
    {
        return await _roleRepository.GetRoleByIdAsync(id);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllRulesAsync();
    }

    public async Task<IEnumerable<Role>> GetRoleByName(String name)
    {
        return await _roleRepository.GetRolesByName(name);
    }

    public async Task AddRoleAsync(Role role)
    {
        await _roleRepository.AddRoleAsync(role);
    }

    public async Task DeleteRoleByIdAsync(Guid id)
    {
        await _roleRepository.DeleteRoleByIdAsync(id);
    }

    public async Task UpdateRoleAsync(Role newRole)
    {
        await _roleRepository.UpdateRoleAsync(newRole);
    }
}