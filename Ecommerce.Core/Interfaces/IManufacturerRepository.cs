using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IManufacturerRepository
{
    public Task<Manufacturer> GetManufacturerByIdAsync(Guid id);
    public Task<Manufacturer> SearchManufacturerByAddressAsync(string address);
    public Task<Manufacturer> SearchManufacturerByEmailAsync(string email);
    public Task<Manufacturer> SearchManufacturerByPhoneNumberAsync(string phoneNumber);
    public Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
    public Task<IEnumerable<Manufacturer>> GetManufacturersByOwnerAsync(string ownerName);
    public Task AddManufacturerAsync(Manufacturer addManufacturer);
    public Task UpdateManufacturerAsync(Manufacturer updateManufacturer);
    public Task<bool> DeleteManufacturerAsync(Guid manufactureId);
    public Task<Manufacturer> GetManufactureProductAsync(Guid manufacturerId);
    
}