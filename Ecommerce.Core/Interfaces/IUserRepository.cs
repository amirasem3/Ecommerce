using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetUsersByNameAsync(string name);
    Task AddUserAsync(User user);
    Task DeleteUserByIdAsync(Guid id);

    Task<IEnumerable<User>> GetUserByRoleAsync(string roleName);
    Task UpdateUserAsync(User newUser);
    

}