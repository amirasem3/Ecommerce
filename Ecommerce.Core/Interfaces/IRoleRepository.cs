using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetRoleByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllRulesAsync();
    Task<Role> GetRoleByName(string name);
    Task AddRoleAsync(Role role);
    Task DeleteRoleByIdAsync(Guid id);
    Task UpdateRoleAsync(Role newRole);
    
}