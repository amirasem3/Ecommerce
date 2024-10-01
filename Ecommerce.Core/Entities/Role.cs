using System.Text.Json.Serialization;
using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Core.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    //N-N relation
    [JsonIgnore]
    public ICollection<UserRole> UserRoles { get; set; }

    public ICollection<User> Users { get; } = new List<User>();
}