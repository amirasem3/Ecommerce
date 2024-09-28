using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Core.Interfaces.RelationRepoInterfaces;

public interface IManufacturerProductRepository
{
    public Task AddManufacturerProductAsync(ManufacturerProduct manufacturerProduct);

    public Task<bool> ManufacturerHaveTheProductAsync(Guid manufacturerId, Guid productId);
}