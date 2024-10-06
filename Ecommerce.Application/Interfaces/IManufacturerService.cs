using Ecommerce.Application.DTOs.Manufacturer;

namespace Ecommerce.Application.Interfaces;

public interface IManufacturerService
{
    public Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id);
    public Task<IEnumerable<ManufacturerDto>> SearchManufacturerByAddressAsync(string address);
    public Task<IEnumerable<ManufacturerDto>> SearchManufacturerByEmailAsync(string email);
    public Task<IEnumerable<ManufacturerDto>> SearchManufacturerByPhoneNumberAsync(string phoneNumber);
    public Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync();
    public Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName);
    public Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto manufacturerDto);
    public Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,AddUpdateManufacturerDto addUpdateManufacturerDto);
    public Task AssignManufacturerProductsAsync(Guid manufacturerID, Guid productID);
    public Task<bool> DeleteManufacturerAsync(Guid manufactureId);
    public Task<ManufacturerDto> GetManufactureProductAsync(Guid manufacturerId);

    public Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId);
}