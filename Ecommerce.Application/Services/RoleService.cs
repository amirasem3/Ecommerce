using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Serilog.Core;

namespace Ecommerce.Application.Services;

public class RoleService
{
    public const string RoleException = "Role Not Found!";
    private readonly UnitOfWork _unitOfWork;


    public RoleService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a role by ID.", args: [id]);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
        {
            LoggerHelper.LogWithDetails(args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        LoggerHelper.LogWithDetails("Role found successfully.", args: [id], retrievedData: role);
        return new RoleDto
        {
            Name = role.Name,
            Id = role.Id,
        };
    }

    public async Task<RoleDto> AddRoleAsync(AddUpdateRoleDto newRole)
    {
        LoggerHelper.LogWithDetails("Attempts to add a new Role", args: [newRole]);
        var role = new Role
        {
            Name = newRole.Name,
            Id = Guid.NewGuid()
        };

        LoggerHelper.LogWithDetails("New Role Created Successfully.");
        await _unitOfWork.RoleRepository.InsertAsync(role);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails("New Role added successfully.", args: [newRole], retrievedData: role);

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto)
    {
        LoggerHelper.LogWithDetails("Attempt to update a role.", args: [id, updateRoleDto]);
        var targetRole = await _unitOfWork.RoleRepository.GetByIdAsync(id);

        if (targetRole == null)
        {
            LoggerHelper.LogWithDetails("There is no user with this ID.", args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }
        LoggerHelper.LogWithDetails("Role Found",args:[id],retrievedData:targetRole);
        targetRole.Name = updateRoleDto.Name;

        _unitOfWork.RoleRepository.Update(targetRole);
        await _unitOfWork.SaveAsync();
        
        LoggerHelper.LogWithDetails("Role Updated Successfully.",args:[id, updateRoleDto], retrievedData:targetRole);

        return new RoleDto
        {
            Name = targetRole.Name,
            Id = targetRole.Id,
        };
    }

    public async Task<bool> DeleteRoleByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to delete a role by ID.",args:[id]);
        var targetRole = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (targetRole == null)
        {
            LoggerHelper.LogWithDetails("There is no role with this ID", args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        await _unitOfWork.RoleRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Delete",args:[id],retrievedData:targetRole);
        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.RoleRepository.GetAsync();
        if (roles == null)
        {
            LoggerHelper.LogWithDetails("Role table is empty.", retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        LoggerHelper.LogWithDetails("All Roles retrieved successfully.", retrievedData: roles);

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        });
    }

    public async Task<RoleDto> GetRoleByNameAsync(string name)
    {
        LoggerHelper.LogWithDetails("Attempts to get a role by name", args: [name]);
        var role = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: name);

        if (role == null)
        {
            LoggerHelper.LogWithDetails("There is no role with this name.", retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        LoggerHelper.LogWithDetails($"Role with name{name} found successfully.", args: [name], retrievedData: role);
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };
    }
}