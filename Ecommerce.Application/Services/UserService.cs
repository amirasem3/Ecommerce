using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(String username)
    {
        return await _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<User> GetUserByEmailAsync(String email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User> GetUserByPhoneNumberAsync(String phoneNumber)
    {
        return await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByNameAsync(String name)
    {
        return await _userRepository.GetUsersByNameAsync(name);
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task DeleteUserByIdAsync(Guid id)
    {
        await _userRepository.DeleteUserByIdAsync(id);
    }

    public async Task UpdateUserAsync(User newUser)
    {
        await _userRepository.UpdateUserAsync(newUser);
    }


}