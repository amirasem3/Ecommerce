using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;


namespace Ecommerce.Application.Services;

public class UserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly UnitOfWork _unitOfWork;
    public const string UserException = "User Not Found!";
    public const string FoundUser = "\n User :";
    public const string Location = "UserService";


    public UserService(IPasswordHasher<User> passwordHasher, UnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempting to retrieve user data by Id.", null!, [id]);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            LoggerHelper.LogWithDetails(args: [id], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }
        LoggerHelper.LogWithDetails($"Attempting to get user({id}) role", args: [user.RoleId]);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        LoggerHelper.LogWithDetails("Role found",args:[user.RoleId],retrievedData:role);
        LoggerHelper.LogWithDetails("User found successfully", args: [id], retrievedData: user);
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
        LoggerHelper.LogWithDetails("Attempting to retrieve user data by username.", null!, [username]);
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Username",
            uniquePropertyValue: username);
        if (user == null)
        {
            LoggerHelper.LogWithDetails(args: [username], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }

        LoggerHelper.LogWithDetails($"Attempting to get user({user.Id}) role", args: [user.RoleId]);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        LoggerHelper.LogWithDetails($"Role Found", args: [user.RoleId], retrievedData: role);
        LoggerHelper.LogWithDetails("User found successfully", args: [username], retrievedData: user);

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
        LoggerHelper.LogWithDetails("Attempting to get user by Email.", args: [email]);

        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Email",
            uniquePropertyValue: email);

        if (user == null)
        {
            LoggerHelper.LogWithDetails(args: [email], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }

        LoggerHelper.LogWithDetails($"Attempting to get user({user.Id}) role", args: [user.RoleId]);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        LoggerHelper.LogWithDetails("Role Found.", args: [user.RoleId], retrievedData: role);
        LoggerHelper.LogWithDetails("User found Successfully.", retrievedData: user, args: [email]);

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
        LoggerHelper.LogWithDetails("Attempting to get user by Phone number.", args: [phoneNumber]);
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "PhoneNumber",
            uniquePropertyValue: phoneNumber);

        if (user == null)
        {
            LoggerHelper.LogWithDetails(args: [phoneNumber], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }

        LoggerHelper.LogWithDetails($"Attempting to get user({user.Id}) role", args: [user.RoleId]);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        LoggerHelper.LogWithDetails("Role Found", args: [user.RoleId], retrievedData: role);
        LoggerHelper.LogWithDetails("User found Successfully.", retrievedData: user, args: [phoneNumber]);
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
        LoggerHelper.LogWithDetails("Attempting to add new user .", args: [registerUserDto]);
        LoggerHelper.LogWithDetails($"Check existence of {registerUserDto.RoleName}");
        var userRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: registerUserDto.RoleName);
        if (userRole == null)
        {
            LoggerHelper.LogWithDetails(args: [registerUserDto.RoleName], retrievedData: RoleService.RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleService.RoleException);
        }

        LoggerHelper.LogWithDetails($"{registerUserDto.RoleName} exists.", retrievedData: userRole);

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

        LoggerHelper.LogWithDetails("New User Created Successfully.", args: [registerUserDto], retrievedData: user);

        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("User Saved Successfully", retrievedData: user);

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
        LoggerHelper.LogWithDetails("Attempt to Update an user.", args: [id, updateUserDto]);
        LoggerHelper.LogWithDetails("Check existence of the User", args: [id]);
        var targetUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (targetUser == null)
        {
            LoggerHelper.LogWithDetails(args: [id], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }

        LoggerHelper.LogWithDetails("User exists", args: [id], retrievedData: targetUser);
        LoggerHelper.LogWithDetails("Check existence of new the Role.", args: [updateUserDto.RoleName]);
        var targetRole = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: updateUserDto.RoleName);
        if (targetRole == null)
        {
            LoggerHelper.LogWithDetails(args: [updateUserDto.RoleName], retrievedData: RoleService.RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleService.RoleException);
        }

        LoggerHelper.LogWithDetails("Role exists", args: [updateUserDto.RoleName], retrievedData: targetRole);

        targetUser.FirstName = updateUserDto.FirstName;
        targetUser.LastName = updateUserDto.LastName;
        targetUser.Email = updateUserDto.Email;
        targetUser.PhoneNumber = updateUserDto.PhoneNumber;
        targetUser.Username = updateUserDto.Username;
        targetUser.PasswordHash = _passwordHasher.HashPassword(targetUser, updateUserDto.Password);
        targetUser.RoleId = targetRole.Id;


        _unitOfWork.UserRepository.Update(targetUser);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails("User Updated Successfully.", args: [id, updateUserDto], retrievedData: targetUser);

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
        LoggerHelper.LogWithDetails("Attempt to Delete an user by ID.", args: [id]);
        LoggerHelper.LogWithDetails("Checking existence of the user.", args: [id]);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            LoggerHelper.LogWithDetails(args: [id], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(UserException);
        }

        LoggerHelper.LogWithDetails("User exists", args: [id], retrievedData: user);

        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails("User Deleted successfully.", args: [id], retrievedData: user);
        return true;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        LoggerHelper.LogWithDetails("Attempt to get all Users.");

        var users = await _unitOfWork.UserRepository.GetAsync();

        if (users == null)
        {
            LoggerHelper.LogWithDetails(message: "User table is empty.", retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("There are no Users");
        }


        LoggerHelper.LogWithDetails("Attempt to get all users' role names");
        var roles = await _unitOfWork.RoleRepository.GetAsync();
        if (roles == null)
        {
            LoggerHelper.LogWithDetails(message: "Role table is empty.", retrievedData: RoleService.RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("There are no roles.");
        }

        LoggerHelper.LogWithDetails("Roles retrieved Successfully", retrievedData: roles);
        LoggerHelper.LogWithDetails("Users Retrieved Successfully", retrievedData: users);
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
        string searchTerm = name;
        LoggerHelper.LogWithDetails("Attempt to search users by its Name", args: [searchTerm]);
        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.FirstName.Contains(name));

        if (users == null)
        {
            LoggerHelper.LogWithDetails(message: "User table is empty or there is no user with this name", args: [searchTerm],
                retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("There is no User!");
        }

        LoggerHelper.LogWithDetails("Attempt to get all users' role names");
        var roles = await _unitOfWork.RoleRepository.GetAsync();

        LoggerHelper.LogWithDetails("Roles retrieved Successfully", retrievedData: roles);
        LoggerHelper.LogWithDetails("Users Retrieved Successfully", retrievedData: users);

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
        LoggerHelper.LogWithDetails("Attempt to get users by role name.", args: [roleName]);
        LoggerHelper.LogWithDetails("Check the role name existence", args: [roleName]);
        var role = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: roleName);

        if (role == null)
        {
            LoggerHelper.LogWithDetails("There is no role with this name.", args: [roleName],
                retrievedData: RoleService.RoleException, logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleService.RoleException);
        }

        LoggerHelper.LogWithDetails("Role exists", args: [roleName], retrievedData: role);
        LoggerHelper.LogWithDetails("Attempt to filter users by this role name", args: [roleName]);
        var users = await _unitOfWork.UserRepository.GetAsync(filter: user => user.RoleId == role.Id);

        if (users == null)
        {
            LoggerHelper.LogWithDetails("There is no user with this role name.", args: [roleName],
                retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Users Not Found!");
        }

        var roles = await _unitOfWork.RoleRepository.GetAsync();

        LoggerHelper.LogWithDetails($"Users which have role named {roleName} successfully found.", args: [roleName],
            retrievedData: users);

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
        LoggerHelper.LogWithDetails("Attempt to authenticate an user", args: [username, password]);
        LoggerHelper.LogWithDetails("Check the user existence", args: [username]);
        var user = await _unitOfWork.UserRepository.GetByUniquePropertyAsync(uniqueProperty: "Username",
            uniquePropertyValue: username);
        if (user == null)
        {
            LoggerHelper.LogWithDetails("Incorrect username.", args: [username], retrievedData: UserException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Incorrect Username!");
        }

        LoggerHelper.LogWithDetails("User exists", args: [username], retrievedData: user);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        if (role == null)
        {
            throw new Exception(user.RoleId.ToString());
        }

        LoggerHelper.LogWithDetails("Attempt to verify the password", args: [password]);
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            LoggerHelper.LogWithDetails("Incorrect Password!", args: [password], logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Incorrect Password!");
        }

        LoggerHelper.LogWithDetails("User authenticated successfully.", args: [username, password],
            retrievedData: user);

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