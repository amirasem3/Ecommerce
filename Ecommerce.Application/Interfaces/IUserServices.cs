using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.User;

namespace Ecommerce.Application.Interfaces;

public interface IUserServices
{
    Task<UserDto> GetUserByIdAsync(Guid id);

    Task<UserDto> GetUserByUsernameAsync(string username);

    Task<UserDto> GetUserByEmailAsync(string email);

    Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber);

    Task<UserDto> AddUserAsync(RegisterUserDto registerUserDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name);

    Task<IEnumerable<UserDto>> GetUserByRoleAsync(string roleName);
    Task<UserDto> AuthenticateUserAsync(string username, string password);
}