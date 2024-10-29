using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Repositories;

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
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
        {
            throw new Exception(RoleException);
        }

        return new RoleDto
        {
            Name = role.Name,
            Id = role.Id,
        };
    }

    public async Task<RoleDto> AddRoleAsync(AddUpdateRoleDto newRole)
    {
        var role = new Role
        {
            Name = newRole.Name,
            Id = Guid.NewGuid()
        };
        await _unitOfWork.RoleRepository.InsertAsync(role);
        await _unitOfWork.SaveAsync();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, AddUpdateRoleDto updateRoleDto)
    {
        var targetRole = await _unitOfWork.RoleRepository.GetByIdAsync(id);

        if (targetRole == null)
        {
            throw new Exception(RoleException);
        }

        targetRole.Name = updateRoleDto.Name;

        _unitOfWork.RoleRepository.Update(targetRole);
        await _unitOfWork.SaveAsync();

        return new RoleDto
        {
            Name = targetRole.Name,
            Id = targetRole.Id,
        };
    }

    public async Task<bool> DeleteRoleByIdAsync(Guid id)
    {
        var targetRole = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (targetRole == null)
        {
            throw new Exception(RoleException);
        }

        await _unitOfWork.RoleRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.RoleRepository.GetAsync();
        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        });
    }

    public async Task<RoleDto> GetRoleByNameAsync(string name)
    {
        var role = await _unitOfWork.RoleRepository.GetByUniquePropertyAsync(uniqueProperty: "Name",
            uniquePropertyValue: name);

        if (role == null)
        {
            throw new Exception(RoleException);
        }

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };
    }
}