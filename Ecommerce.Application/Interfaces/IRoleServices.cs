using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IRoleServices
{
    Task<RoleDto> GetRoleByIdAsync(Guid id);
    Task<RoleDto> AddRoleAsync( AddUpdateRoleDto role);
    Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto);
    Task<bool> DeleteRoleByIdAsync(Guid id);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto> GetRoleByNameAsync(string name);
}