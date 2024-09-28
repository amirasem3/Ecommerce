using Ecommerce.Application.DTOs.Manufacturer;

namespace Ecommerce.Application.Interfaces;

public interface IManufacturerService
{
    public Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id);
    public Task<ManufacturerDto> SearchManufacturerByAddressAsync(string address);
    public Task<ManufacturerDto> SearchManufacturerByEmailAsync(string email);
    public Task<ManufacturerDto> SearchManufacturerByPhoneNumberAsync(string phoneNumber);
    public Task<IEnumerable<ManufacturerDto>> GetAllManufacturersById();
    public Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName);
    public Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto addManufacturerDto);
    public Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,AddUpdateManufacturerDto updateManufacturerDto);
    public Task AssignProductToManufacturerAsync(Guid manufacturerID, Guid productID);
    public Task<bool> DeleteManufacturerAsync(Guid manufactureId);
    public Task<ManufacturerDto> GetManufactureProductAsync(Guid manufacturerId);
}