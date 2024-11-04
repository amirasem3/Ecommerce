using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Ecommerce.Application.Services;

public class RoleService
{
    public const string RoleException = "Role Not Found!";
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<RoleService> _logger;


    public RoleService(UnitOfWork unitOfWork,ILogger<RoleService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a role by ID.", args: [id]);
        var role = await _unitOfWork.roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }
        var roleRes =new RoleDto
        {
            Name = role.Name,
            Id = role.Id,
        }; 
        LoggerHelper.LogWithDetails(_logger,"Target Role Found",args:[id], retrievedData:roleRes);
        return roleRes; 
    }

    public async Task<RoleDto> AddRoleAsync(AddUpdateRoleDto newRole) 
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to add a new Role", args: [newRole]);
        var role = new Role
        {
            Name = newRole.Name,
            Id = Guid.NewGuid()
        };

        LoggerHelper.LogWithDetails(_logger,"New Role Created Successfully.");
        await _unitOfWork.roleRepository.InsertAsync(role);
        await _unitOfWork.SaveAsync();


        var roleRes = new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
        LoggerHelper.LogWithDetails(_logger,"New Role added successfully.", args: [newRole], retrievedData: roleRes);
        return roleRes;
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to update a role.", args: [id, updateRoleDto]);
        var targetRole = await _unitOfWork.roleRepository.GetByIdAsync(id);

        if (targetRole == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no user with this ID.", args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }
        LoggerHelper.LogWithDetails(_logger,"Role Found",args:[id],retrievedData:targetRole);
        targetRole.Name = updateRoleDto.Name;

        _unitOfWork.roleRepository.Update(targetRole);
        await _unitOfWork.SaveAsync();
        
        var roleRes  =new RoleDto
        {
            Name = targetRole.Name,
            Id = targetRole.Id,
        };
        LoggerHelper.LogWithDetails(_logger,"Role Updated Successfully.", args: [id, updateRoleDto], retrievedData: roleRes);
        return roleRes;
    }

    public async Task<bool> DeleteRoleByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to delete a role by ID.",args:[id]);
        var targetRole = await _unitOfWork.roleRepository.GetByIdAsync(id);
        if (targetRole == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no role with this ID", args: [id], retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        await _unitOfWork.roleRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Delete",args:[id],retrievedData:targetRole);
        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get all roles");
        var roles = await _unitOfWork.roleRepository.GetAsync();
        if (roles == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Role table is empty.", retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        var roleRes =roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        });
        LoggerHelper.LogWithDetails(_logger,"All Roles",retrievedData:roleRes);
        return roleRes;
    }

    public async Task<RoleDto> GetRoleByNameAsync(string name)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to get a role by name", args: [name]);
        var role = await _unitOfWork.roleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: name);

        if (role == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no role with this name.", retrievedData: RoleException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(RoleException);
        }

        var roleRes = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };
        LoggerHelper.LogWithDetails(_logger,$"Role with name{name} found successfully.", args: [name], retrievedData: roleRes);

        return roleRes;
    }
}