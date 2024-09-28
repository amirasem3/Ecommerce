using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }   
    
    public string PasswordHash { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
    
}