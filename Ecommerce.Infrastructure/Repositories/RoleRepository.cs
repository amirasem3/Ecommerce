using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly EcommerceDbContext _context;
    
    public RoleRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task<Role> GetRoleByIdAsync(Guid id)
    {
        return (await _context.Roles.FindAsync(id))!;
    }

    public async Task<IEnumerable<Role>> GetAllRulesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role> GetRoleByName(string name)
    {
        if (name == " ")
        {
            return null!;
        }

        return   (await _context.Roles.FirstOrDefaultAsync(role => role.Name == name))!;
    }

    public async Task AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoleByIdAsync(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        _context.Roles.Remove(role!);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(Role newRole)
    {
        _context.Roles.Update(newRole);
        await _context.SaveChangesAsync();
    }
    
}