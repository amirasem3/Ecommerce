using System.Text.Json.Serialization;
using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    //N-N Relation with Role
    [JsonIgnore]
    public ICollection<UserRole> UserRoles { get; set; }
    
}