using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Core.Interfaces.RelationRepoInterfaces;

public interface IUserRoleRepository
{
    public Task AddUserRoleAsync(UserRole userRole);

    public Task<bool> UserHaveTheRoleAsync(Guid roleId, Guid userId);
}