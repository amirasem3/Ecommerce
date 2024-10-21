using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Services;

public class UserService : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    public const string UserException = "User Not Found!";
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new Exception(UserException);
        }
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(RoleService.RoleException);
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(RoleService.RoleException);
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(RoleService.RoleException);
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(RoleService.RoleException);
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name,
        };
    }

    public async Task<UserDto> AddUserAsync(AddUpdateUserDto registerUserDto)
    {
        var userRole = await _roleRepository.GetRoleByName(registerUserDto.RoleName);
        if (userRole == null)
        {
            throw new Exception(RoleService.RoleException);
        }
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email,
            PhoneNumber = registerUserDto.PhoneNumber,
            Username = registerUserDto.Username,
            RoleId = userRole.Id
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);
        await _userRepository.AddUserAsync(user);
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = user.PasswordHash,
            RoleName = userRole.Name,
            RoleId = user.RoleId,
        };
    }


    public async Task<UserDto> UpdateUserAsync(Guid id, AddUpdateUserDto updateUserDto)
    {
        var targetUser = await _userRepository.GetUserByIdAsync(id);
        if (targetUser == null)
        {
            throw new Exception(UserException);
        }
        var targetRole = await _roleRepository.GetRoleByName(updateUserDto.RoleName);
        if (targetRole == null)
        {
            throw new Exception(RoleService.RoleException);
        }
        targetUser.FirstName = updateUserDto.FirstName;
        targetUser.LastName = updateUserDto.LastName;
        targetUser.Email = updateUserDto.Email;
        targetUser.PhoneNumber = updateUserDto.PhoneNumber;
        targetUser.Username = updateUserDto.Username;
        targetUser.PasswordHash = _passwordHasher.HashPassword(targetUser, updateUserDto.Password);
        targetUser.RoleId = targetRole.Id;


        await _userRepository.UpdateUserAsync(targetUser);

        return new UserDto
        {
            Id = targetUser.Id,
            FirstName = targetUser.FirstName,
            LastName = targetUser.LastName,
            Email = targetUser.Email,
            PhoneNumber = targetUser.PhoneNumber,
            PasswordHash = targetUser.PasswordHash,
            Username = targetUser.Username,
            RoleName = targetRole.Name,
            RoleId = targetRole.Id
        };
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            throw new Exception(UserException);
        }

        await _userRepository.DeleteUserByIdAsync(id);
        return true;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var roles = await _roleRepository.GetAllRulesAsync();
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = roles.FirstOrDefault(role => role.Id == user.RoleId)!.Name
        });
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name)
    {
        var users = await _userRepository.GetUsersByNameAsync(name);
        if (users == null)
        {
            throw new Exception("Users Not Found!");
        }
        var roles = await _roleRepository.GetAllRulesAsync();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = roles.FirstOrDefault(role => role.Id == user.RoleId)!.Name
        });
    }

    public async Task<IEnumerable<UserDto>> GetUserByRoleAsync(string roleName)
    {
        var users = await _userRepository.GetUserByRoleAsync(roleName);
        if (users==null)
        {
            throw new Exception("Users Not Found!");
        }
        var roles = await _roleRepository.GetAllRulesAsync();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = roles.FirstOrDefault(role => role.Id == user.RoleId)!.Name
        });
    }


    public async Task<UserDto> AuthenticateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception("Incorrect Username!");
        }

        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);


        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new Exception("Incorrect Password!");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }
}