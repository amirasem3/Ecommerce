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
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

        
        
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles, 
            RoleId = user.RoleId,
            RoleName = role.Name
            
           
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        
        return new UserDto
        {
          Id  = user.Id,
          FirstName = user.FirstName,
          LastName = user.LastName,
          Username = user.Username,
          Email = user.Email,
          PhoneNumber = user.PhoneNumber,
          UserRoles = user.UserRoles,
          RoleId = user.RoleId,
          RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles,
            RoleId = user.RoleId,
            RoleName = role.Name
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
            RoleId =targetRole.Id
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
        return users.Select(user => new UserDto{
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                UserRoles = user.UserRoles,
                RoleId = user.RoleId,
            });

        

        

    }

    public async Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name)
    {
        var users = await _userRepository.GetUsersByNameAsync(name);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            UserRoles = user.UserRoles,
            RoleName = user.Role.Name,
            RoleId = user.RoleId,
        });
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
        user.RoleId = role.Id;
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
        var user = await _userRepository.GetUserByIdAsync(userId);
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
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
            UserRoles = user2.UserRoles,
            RoleId = user.RoleId
           
        };
    }
}