using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Services;

public class UserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly UnitOfWork _unitOfWork;
    public const string UserException = "User Not Found!";

    public UserService(IPasswordHasher<User> passwordHasher, UnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
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
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId,
            RoleName = role.Name
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Username",
            uniquePropertyValue: username);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
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

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Email",
            uniquePropertyValue: email);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
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
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "PhoneNumber",
            uniquePropertyValue: phoneNumber);
        if (user == null)
        {
            throw new Exception(UserException);
        }

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
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
        var userRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: registerUserDto.RoleName);
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
        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveAsync();
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
        var targetUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (targetUser == null)
        {
            throw new Exception(UserException);
        }

        var targetRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: updateUserDto.RoleName);
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

        _unitOfWork.UserRepository.Update(targetUser);
        await _unitOfWork.SaveAsync();

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
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception(UserException);
        }

        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAsync();
        var roles = await _unitOfWork.RoleRepository.GetAsync();

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
        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.FirstName.Contains(name));
        if (users == null)
        {
            throw new Exception("Users Not Found!");
        }

        var roles = await _unitOfWork.RoleRepository.GetAsync();

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
        var role = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: roleName);
        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.RoleId == role.Id);
        if (users == null)
        {
            throw new Exception("Users Not Found!");
        }

        var roles = await _unitOfWork.RoleRepository.GetAsync();

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