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
    private readonly IPasswordHasher<User> _passwordHasher;
    private IEnumerable<User> _users = new List<User>();
    private IEnumerable<Role> _roles = new List<Role>();
    private int counter = 0;

    public  UserService(IUserRepository userRepository, IRoleRepository roleRepository,  IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
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
            RoleId = user.RoleId,
            RoleName = role.Name
            
           
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user==null)
        {
            return null;
        }
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        
        return new UserDto
        {
          Id  = user.Id,
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
        if (user==null)
        {
            return null;
        }
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

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

    public async Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        if (user == null)
        {
            return null;
        }
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = role.Name,
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
            return null;
        }
        var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);
        

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
            RoleId = user.RoleId,
            RoleName = role.Name
           
        };
    }

    public async Task<bool> IsUserExistAsync(string identifier)
    {
        return await _userRepository.IsUserExist(identifier);
    }
}