using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Application.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
    
    
}