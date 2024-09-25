using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(String username);
    Task<User> GetUserByEmailAsync(String email);
    Task<User> GetUserByPhoneNumberAsync(String phoneNumber);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetUsersByNameAsync(String name);
    Task AddUserAsync(User user);
    Task DeleteUserByIdAsync(Guid id);
    Task UpdateUserAsync(User newUser);

}