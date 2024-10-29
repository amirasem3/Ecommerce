using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using EcommerceSolution.Logs;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Ecommerce.Application.Services;

public class UserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly UnitOfWork _unitOfWork;
    public const string UserException = "User Not Found!";
    public const string FoundUser = "\n User :";
    public const string Location = "UserService";


    private readonly LoggerHelper<UserService> _logger;

    public UserService(IPasswordHasher<User> passwordHasher, UnitOfWork unitOfWork, LoggerHelper<UserService> logger)
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        _logger.Log(0, "User", 0, input: $"{id}");
        _logger.Log(0, "User", 4, input: $"{id}", repoFunctionIndex: 0, repoEntity: "User");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        _logger.Log(0, "User", 5, output: $"{user}", repoFunctionIndex: 0, repoEntity: "User");
        if (user == null)
        {
            _logger.Log(0, "User", 7, message: "Unsuccessful Search!", repoEntity: "User",
                repoFunctionIndex: 0);
            _logger.Log(0, "User", 7, message: $"User with Id {id} not found.", repoEntity: "User"
                , repoFunctionIndex: 0);
            throw new Exception(UserException);
        }

        _logger.Log(0, "User", 4, input: $"{user.RoleId}", repoFunctionIndex: 0, repoEntity: "Role");
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        _logger.Log(0, "User", 5, output: role.ToString(), repoEntity: "Role");
        if (role == null)
        {
            _logger.Log(0, "User", 7, message: "Unsuccessful Search!", repoEntity: "Role", repoFunctionIndex: 0);
            _logger.Log(0, "User", 7, message: $"Role with Id {user.RoleId} not found for User ID {user.Id}.",
                repoEntity: "Role", repoFunctionIndex: 0);
            throw new Exception(RoleService.RoleException);
        }

        _logger.Log(0, "User", 2, message: $"User with ID {id} found successfully.");
        _logger.Log(0, "User", 2, message: $"Output  {FoundUser} {user}");

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        _logger.Log(1, "User", 0, input: $"{username}");
        _logger.Log(1, "User", 4, input: $"uniqueProperty: \"Username\",uniquePropertyValue: {username}",
            repoFunctionIndex: 1, repoEntity: "User");
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Username",
            uniquePropertyValue: username);
        _logger.Log(1, "User", 5, output: user.ToString(),
            repoFunctionIndex: 1, repoEntity: "User");
        if (user == null)
        {
            _logger.Log(1, "User", 7, message: "Unsuccessful Search!", repoEntity: "User",
                repoFunctionIndex: 1);
            _logger.Log(1, "User", 7, message: $" User with Username {username} not found.", repoEntity: "User",
                repoFunctionIndex: 1);
            throw new Exception(UserException);
        }

        _logger.Log(1, "User", 4, input: $"{user.RoleId}", repoFunctionIndex: 0, repoEntity: "Role");
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        _logger.Log(1, "User", 5, output: role.ToString(), repoEntity: "Role");
        if (role == null)
        {
            _logger.Log(1, "User", 7, message: $" Role with RoleID {user.RoleId} not found!", repoEntity: "Role",
                repoFunctionIndex: 0);
            _logger.Log(1, "User", 7, message: $"Role with Id {user.RoleId} not found for User ID {user.Id}.",
                repoEntity: "Role", repoFunctionIndex: 0);
            throw new Exception(RoleService.RoleException);
        }


        _logger.Log(1, "User", 2, message: $"User with Username {username} found successfully.");
        _logger.Log(1, "User", 2, message: $"{FoundUser} {user}");

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

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        _logger.Log(2, "User", 0, input: $"{email}");
        _logger.Log(2, "User", 4, input: $"uniqueProperty: \"Email\",uniquePropertyValue: {email}",
            repoFunctionIndex: 1, repoEntity: "User");

        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Email",
            uniquePropertyValue: email);

        _logger.Log(2, "User", 5, output: user.ToString(),
            repoFunctionIndex: 1, repoEntity: "User");

        if (user == null)
        {
            _logger.Log(2, "User", 7, message: "Unsuccessful Search!", repoEntity: "User",
                repoFunctionIndex: 1);
            _logger.Log(2, "User", 7, message: $" User with Email {email} not found.", repoEntity: "User",
                repoFunctionIndex: 1);
            throw new Exception(UserException);
        }

        _logger.Log(2, "User", 4, input: $"{user.RoleId}", repoFunctionIndex: 0, repoEntity: "Role");

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);

        _logger.Log(2, "User", 5, output: role.ToString(), repoEntity: "Role");

        if (role == null)
        {
            _logger.Log(2, "User", 7, message: $" Role with RoleID {user.RoleId} not found!", repoEntity: "Role",
                repoFunctionIndex: 0);
            _logger.Log(2, "User", 7, message: $"Role with Id {user.RoleId} not found for User ID {user.Id}.",
                repoEntity: "Role", repoFunctionIndex: 0);
            throw new Exception(RoleService.RoleException);
        }


        _logger.Log(2, "User", 2, message: $"User with Email {email} found successfully.");
        _logger.Log(2, "User", 2, message: $"{FoundUser} {user}");
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
        _logger.Log(3, "User", 0, input: $"{phoneNumber}");
        _logger.Log(3, "User", 4, input: $"uniqueProperty: \"PhoneNumber\",uniquePropertyValue: {phoneNumber}",
            repoFunctionIndex: 1, repoEntity: "User");

        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "PhoneNumber",
            uniquePropertyValue: phoneNumber);

        _logger.Log(3, "User", 5, output: user.ToString(),
            repoFunctionIndex: 1, repoEntity: "User");
        if (user == null)
        {
            _logger.Log(3, "User", 7, message: "Unsuccessful Search!", repoEntity: "User",
                repoFunctionIndex: 1);
            _logger.Log(3, "User", 7, message: $" User with Phone number {phoneNumber} not found.", repoEntity: "User",
                repoFunctionIndex: 1);
            throw new Exception(UserException);
        }

        _logger.Log(3, "User", 4, input: $"{user.RoleId}", repoFunctionIndex: 0, repoEntity: "Role");

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);

        _logger.Log(3, "User", 5, output: role.ToString(), repoEntity: "Role");

        if (role == null)
        {
            _logger.Log(3, "User", 7, message: $" Role with RoleID {user.RoleId} not found!", repoEntity: "Role",
                repoFunctionIndex: 0);
            _logger.Log(3, "User", 7, message: $"Role with Id {user.RoleId} not found for User ID {user.Id}.",
                repoEntity: "Role", repoFunctionIndex: 0);
            throw new Exception(RoleService.RoleException);
        }


        _logger.Log(3, "User", 2, message: $"User with phone number {phoneNumber} found successfully.");
        _logger.Log(3, "User", 2, message: $"{FoundUser} {user}");
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
        _logger.Log(4, "User", 0, input: $"{registerUserDto}");
        _logger.Log(4, "User", 4, input: $"uniqueProperty: \"Name\",uniquePropertyValue: {registerUserDto.RoleName}",
            repoFunctionIndex: 1, repoEntity: "Role");

        var userRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: registerUserDto.RoleName);

        _logger.Log(4, "User", 5, output: userRole.ToString(),
            repoFunctionIndex: 1, repoEntity: "Role");
        if (userRole == null)
        {
            _logger.Log(4, "User", 7, message: "Unsuccessful Add.", repoEntity: "Role",
                repoFunctionIndex: 1);
            _logger.Log(4, "User", 7, message: $"Role with name {registerUserDto.RoleName} not found!",
                repoEntity: "Role",
                repoFunctionIndex: 1);
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

        _logger.Log(4, "User", 2, message: $"New User created: {user}");

        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveAsync();

        _logger.Log(4, "User", 2, message: "The user Added successfully.");
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
        _logger.Log(5, "User", 0, input: $"{updateUserDto}");
        _logger.Log(5, "User", 4, input: $"{id}",
            repoFunctionIndex: 0, repoEntity: "User");

        var targetUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

        _logger.Log(5, "User", 5, output: $"{targetUser}",
            repoFunctionIndex: 0, repoEntity: "User");


        if (targetUser == null)
        {
            _logger.Log(5, "User", 7, message: "Unsuccessful Update!", repoEntity: "User",
                repoFunctionIndex: 0);
            _logger.Log(5, "User", 7, message: $" User with ID {id} not found.", repoEntity: "User",
                repoFunctionIndex: 0);
            throw new Exception(UserException);
        }

        _logger.Log(5, "User", 4, input: $"uniqueProperty: \"Name\",uniquePropertyValue: {updateUserDto.RoleName}",
            repoFunctionIndex: 1, repoEntity: "Role");
        var targetRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: updateUserDto.RoleName);

        _logger.Log(5, "User", 5, output: $"{targetRole}",
            repoFunctionIndex: 1, repoEntity: "Role");

        if (targetRole == null)
        {
            _logger.Log(5, "User", 7, message: $" Role with RoleID {targetUser.RoleId} not found!", repoEntity: "Role",
                repoFunctionIndex: 1);
            _logger.Log(5, "User", 3, message: $"Role with name {updateUserDto.RoleName} not found!");
            throw new Exception(RoleService.RoleException);
        }

        targetUser.FirstName = updateUserDto.FirstName;
        targetUser.LastName = updateUserDto.LastName;
        targetUser.Email = updateUserDto.Email;
        targetUser.PhoneNumber = updateUserDto.PhoneNumber;
        targetUser.Username = updateUserDto.Username;
        targetUser.PasswordHash = _passwordHasher.HashPassword(targetUser, updateUserDto.Password);
        targetUser.RoleId = targetRole.Id;


        _logger.Log(5, "User", 2, message: $"Updated User {id} data : {targetUser}");

        _unitOfWork.UserRepository.Update(targetUser);
        await _unitOfWork.SaveAsync();

        _logger.Log(5, "User", 2, message: $"The User with Id {id} updated successfully.");

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
        _logger.Log(6, "User", 0, input: $"{id}");
        _logger.Log(6, "User", 4, input: $"{id}",
            repoFunctionIndex: 0, repoEntity: "User");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);


        _logger.Log(6, "User", 5, output: $"{user}",
            repoFunctionIndex: 0, repoEntity: "User");


        if (user == null)
        {
            _logger.Log(6, "User", 7, message: "Unsuccessful Delete!", repoEntity: "User",
                repoFunctionIndex: 0);
            _logger.Log(6, "User", 7, message: $" User with ID {id} not found.", repoEntity: "User",
                repoFunctionIndex: 0);
            throw new Exception(UserException);
        }


        _logger.Log(6, "User", 2, message: $"{FoundUser} {user}");


        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();

        _logger.Log(6, "User", 2, message: $"User with ID {id} deleted Successfully.");
        return true;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        _logger.Log(7, "User", 0);
        _logger.Log(7, "User", 4, repoFunctionIndex: 2, repoEntity: "User");

        var users = await _unitOfWork.UserRepository.GetAsync();

        _logger.Log(7, "User", 5, repoFunctionIndex: 2, repoEntity: "User");
        if (users == null)
        {
            _logger.Log(7, "User", 7, message: "There is no User.", repoEntity: "User");
            throw new Exception("There is no User");
        }


        _logger.Log(7, "User", 4, repoFunctionIndex: 2, repoEntity: "Role");

        var roles = await _unitOfWork.RoleRepository.GetAsync();
        _logger.Log(7, "User", 5, repoFunctionIndex: 2, repoEntity: "Role");
        if (roles == null)
        {
            _logger.Log(7, "User", 7, message: "There is no Role.", repoEntity: "Role");
            throw new Exception("There is no roles.");
        }


        _logger.Log(7, "User", 2, message: "All users retrieved successfully.");
        return users.Select(user => new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            PasswordHash = user.PasswordHash,
            RoleName = roles.FirstOrDefault(role => role.Id == user.RoleId)!.Name
        });
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersByNameAsync(string name)
    {
        _logger.Log(8, "User", 0, input: $"{name}");
        _logger.Log(8, "User", 4, repoFunctionIndex: 2, repoEntity: "User",
            input: $"filter: user => user.FirstName.Contains({name})");

        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.FirstName.Contains(name));
        
        
        _logger.Log(8, "User", 5, repoFunctionIndex: 2, repoEntity: "User",
            output: $"All users, have {name} in their firstnames, fetched successfully.");
        
        if (users == null)
        {
            _logger.Log(8,"User",3,message:"There is no user.");
            throw new Exception("There is no User!");
        }

        _logger.Log(8, "User", 4, repoFunctionIndex: 2, repoEntity: "Role");
        
        var roles = await _unitOfWork.RoleRepository.GetAsync();
        
        _logger.Log(8, "User", 5, repoFunctionIndex: 2, repoEntity: "Role",
            output: "All roles fetched successfully.");
      
        _logger.Log(8, "User",2,message:$"All users, have {name} in their firstnames, retrieved successfully.");
   
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            PasswordHash = user.PasswordHash,
            RoleName = roles.FirstOrDefault(role => role.Id == user.RoleId)!.Name
        });
    }

    public async Task<IEnumerable<UserDto>> GetUserByRoleAsync(string roleName)
    {
        
        _logger.Log(9, "User", 0, input: $"{roleName}");
        _logger.Log(9, "User", 4, repoFunctionIndex: 1, repoEntity: "Role",
            input: $"uniqueProperty: \"Name\",uniquePropertyValue: {roleName}");
        
        var role = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: roleName);
        
        _logger.Log(9, "User", 5, repoFunctionIndex: 1, repoEntity: "Role",
            output: $"{role}");
        
        
        _logger.Log(9, "User", 4, repoFunctionIndex: 2, repoEntity: "User",
            input: $"filter: user => user.RoleId == role.Id");
        
        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.RoleId == role.Id);
        
        _logger.Log(9, "User", 5, repoFunctionIndex: 2, repoEntity: "User",
            output: $"All User fetched that have the condition");
        if (users == null)
        {
            _logger.Log(9, "User", 3,message:"Users Not Found!");
            throw new Exception("Users Not Found!");
        }

        _logger.Log(9, "User", 4, repoFunctionIndex: 2, repoEntity: "Role");
        var roles = await _unitOfWork.RoleRepository.GetAsync();
        _logger.Log(9, "User", 5, repoFunctionIndex: 2, repoEntity: "Role",
            output:"All roles fetched");

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
            RoleName = roles.FirstOrDefault(r => r.Id == user.RoleId)!.Name
        });
    }


    public async Task<UserDto> AuthenticateUserAsync(string username, string password)
    {
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Username",
            uniquePropertyValue: username);
        if (user == null)
        {
            throw new Exception("Incorrect Username!");
        }

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(user.RoleId.ToString());
        }


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