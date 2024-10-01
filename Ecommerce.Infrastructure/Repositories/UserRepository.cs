using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
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

        if (deleted!= null)
        {
            _context.Users.Remove(deleted);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User newUser)
    {
        _context.Users.Update(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<Role> GetUserRole(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        // return (await _context.Users
        //     .Include(u => u.UserRoles)
        //     .ThenInclude(ur => ur.Role)
        //     .FirstOrDefaultAsync(u => u.Id == userId))!;
        var role = user.Role;

        return await _context.Roles.FindAsync(role.Id);
    }
}