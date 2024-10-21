using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IManufacturerRepository
{
    
    public Task AddManufacturerProduct(Manufacturer manufacturer, Product product);
    public Task DeleteManufacturerProductAsync(Manufacturer manufacturer, Product product);
    public Task<Manufacturer> GetManufacturerByIdAsync(Guid id);
    public Task<Manufacturer> GetManufacturerByAddressAsync(string address);
    public Task<Manufacturer> GetManufacturerByEmailAsync(string email);
    public Task<Manufacturer> GetManufacturerByPhoneNumberAsync(string phoneNumber);
    public Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
    public Task<IEnumerable<Manufacturer>> GetManufacturersByOwnerAsync(string ownerName);
    public Task AddManufacturerAsync(Manufacturer addManufacturer);
    public Task UpdateManufacturerAsync(Manufacturer updateManufacturer);
    public Task<bool> DeleteManufacturerAsync(Guid manufactureId);

    
    // public Task<Manufacturer> GetManufactureProductAsync(Guid manufacturerId);

    

}