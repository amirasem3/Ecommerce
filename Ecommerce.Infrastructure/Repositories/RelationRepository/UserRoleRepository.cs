using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories.RelationRepository;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly EcommerceDbContext _context;

    public UserRoleRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task AddUserRoleAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UserHaveTheRoleAsync(Guid roleId, Guid userId)
    {
        
        
        return await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
}