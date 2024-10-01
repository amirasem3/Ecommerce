using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Services;

public class UserService : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        
        
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles, 
            RoleName = user.Role.Name
           
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        
        return new UserDto
        {
          Id  = user.Id,
          FirstName = user.FirstName,
          LastName = user.LastName,
          Username = user.Username,
          Email = user.Email,
          PhoneNumber = user.PhoneNumber,
          UserRoles = user.UserRoles
        };
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles
        };
    }

    public async Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles
        };
    }

    public async Task<UserDto> AddUserAsync(RegisterUserDto registerUserDto)
    {
        var userRole = await _roleRepository.GetRoleByName(registerUserDto.RoleName);
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email,
            PhoneNumber = registerUserDto.PhoneNumber,
            Username = registerUserDto.Username,
            Role = userRole
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
            Role = user.Role
            
        };

    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var targetUser = await _userRepository.GetUserByIdAsync(id);
        var targetRole = await _roleRepository.GetRoleByName(updateUserDto.RoleName);
        targetUser.FirstName = updateUserDto.FirstName;
        targetUser.LastName = updateUserDto.LastName;
        targetUser.Email = updateUserDto.Email;
        targetUser.PhoneNumber = updateUserDto.PhoneNumber;
        targetUser.Username = updateUserDto.Username;
        if (updateUserDto.Password != "")
        {
            targetUser.PasswordHash = updateUserDto.Password;
        }

        targetUser.UserRoles = updateUserDto.UserRoles;

        await _userRepository.UpdateUserAsync(targetUser);

        return new UserDto
        {
            FirstName = targetUser.FirstName,
            LastName = targetUser.LastName,
            Email = targetUser.Email,
            PhoneNumber = targetUser.PhoneNumber,
            Username = targetUser.Username,
            UserRoles = targetUser.UserRoles,
            RoleName = targetRole.Name,
        };
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user != null)
        {
            await _userRepository.DeleteUserByIdAsync(id);
            return true;
        }

        return false;


    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users==null)
        {
            return null;
        }

        IEnumerable<UserDto> result = new List<UserDto>();
        foreach (var user in users)
        {
            var role = user.Role;
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                UserRoles = user.UserRoles,
                RoleName = role.Name
            };
            result.Append(userDto);
        }

        return result;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name)
    {
        var users = await _userRepository.GetUsersByNameAsync(name);

        IEnumerable<UserDto> result = new List<UserDto>();
        foreach (var user in users)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                UserRoles = user.UserRoles,
                RoleName = user.Role.Name
            };
            result.Append(userDto);
        }

        return result;
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (role == null)
        {
            throw new ArgumentException("Role not found.");
        }

        user.Role = role;
        await _userRepository.UpdateUserAsync(user);
        // var exists = await _userRoleRepository.UserHaveTheRoleAsync(roleId, userId);
        // // Check if the user already has this role
        // if (!exists)
        // {
        //     var userRole = new UserRole
        //     {
        //         UserId = userId,
        //         RoleId = roleId
        //     };
        //
        //     await _userRoleRepository.AddUserRoleAsync(userRole);
        //
        // }
    }

    public async Task<UserDto> GetUserRoleAsync(Guid userId)
    {
        var role = await _userRepository.GetUserRole(userId);
        var user = await _userRepository.GetUserByIdAsync(userId);
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            // UserRoles = user.UserRoles,
            Role = role,
            RoleName = role.Name,
        };
    }
    
    public async Task<UserDto> AuthenticateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        var user2 = await _userRepository.GetUserRole(user.Id);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
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
            RoleName = user2.Name,
            UserRoles = user2.UserRoles
           
        };
    }
}