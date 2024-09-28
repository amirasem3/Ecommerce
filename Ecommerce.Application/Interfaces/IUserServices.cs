using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.User;

namespace Ecommerce.Application.Interfaces;

public interface IUserServices
{
    Task AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task<UserDto> GetUserByIdAsync(Guid id);

    Task<UserDto> GetUserByUsernameAsync(string username);

    Task<UserDto> GetUserByEmailAsync(string email);

    Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber);

    Task<UserDto> AddUserAsync(RegisterUserDto registerUserDto);
    Task<UserDto> UpdateProductAsync(Guid id, UpdateUserDto updateUserDto);
    Task<UserDto> GetUserRoleAsync(Guid userId);
    Task<bool> DeleteUserAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name);
    Task<UserDto> AuthenticateUserAsync(string username, string password);
}