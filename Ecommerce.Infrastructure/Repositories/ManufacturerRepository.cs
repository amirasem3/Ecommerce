using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly EcommerceDbContext _context;

    public ManufacturerRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task<Manufacturer> GetManufacturerByIdAsync(Guid id)
    {
        return await _context.Manufacturers.FindAsync(id);
    }

    public async Task<Manufacturer> SearchManufacturerByAddressAsync(string address)
    {
        return await _context.Manufacturers.FirstOrDefaultAsync(m => m.Address == address);

    }

    public async Task<Manufacturer> SearchManufacturerByEmailAsync(string email)
    {
        return await _context.Manufacturers.FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<Manufacturer> SearchManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Manufacturers.FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber);
    }

    public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
    {
        return await _context.Manufacturers.ToListAsync();
    }

    public async Task<IEnumerable<Manufacturer>> GetManufacturersByOwnerAsync(string ownerName)
    {
        return await _context.Manufacturers.Where(u => u.OwnerName == ownerName).ToListAsync();
    }

    public async Task AddManufacturerAsync(Manufacturer addManufacturer)
    {
        await _context.Manufacturers.AddAsync(addManufacturer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateManufacturerAsync(Manufacturer updateManufacturer)
    {
        _context.Manufacturers.Update(updateManufacturer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        var deleted = await _context.Manufacturers.FindAsync(manufactureId);
        if (deleted!=null)
        {
            _context.Manufacturers.Remove(deleted);

            return true;
        }

        return false;

    }

    public async Task<Manufacturer> GetManufactureProductAsync(Guid manufacturerId)
    {
        return (await _context.Manufacturers
            .Include(p => p.Products)
            .ThenInclude(mp => mp.ProductId)
            .FirstOrDefaultAsync(m => m.Id == manufacturerId))!;
    }
}