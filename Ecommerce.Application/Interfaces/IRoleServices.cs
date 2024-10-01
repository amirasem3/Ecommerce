using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IRoleServices
{
    Task<RoleDto> GetRoleByIdAsync(Guid id);
    Task<RoleDto> AddRoleAsync( AddUpdateRoleDto roleDto);
    Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto);
    Task<bool> DeleteRoleByIdAsync(Guid id);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<IEnumerable<RoleDto>> GetAllRolesByNameAsync(string name);
}