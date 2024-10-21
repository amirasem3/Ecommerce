using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EcommerceDbContext _context;

    public UserRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return (await _context.Users.FindAsync(id))!;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Username == username))!;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))!;
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber))!;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
    {
        return await _context.Users.Where(user => user.FirstName.Contains(name)).ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserByIdAsync(Guid id)
    {
        var deleted = await _context.Users.FindAsync(id);

        if (deleted != null)
        {
            _context.Users.Remove(deleted);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserExist(string identifier)
    {
        return await _context.Users.AnyAsync(user => user.Username == identifier || user.Email == identifier);
    }

    public async Task<IEnumerable<User>> GetUserByRoleAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);
        return await _context.Users.Where(user => user.RoleId == role!.Id).ToListAsync();
    }

    public async Task UpdateUserAsync(User newUser)
    {
        _context.Users.Update(newUser);
        await _context.SaveChangesAsync();
    }
}