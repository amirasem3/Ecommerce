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

    public async Task<IEnumerable<Manufacturer>> SearchManufacturerByAddressAsync(string address)
    {
        return await _context.Manufacturers.Where(m => m.Address.Contains(address)).ToListAsync();

    }

    public async Task<IEnumerable<Manufacturer>> SearchManufacturerByEmailAsync(string email)
    {
        return await _context.Manufacturers.Where(m => m.Email.Contains(email)).ToListAsync();
    }

    public async Task<IEnumerable<Manufacturer>> SearchManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Manufacturers.Where(m => m.PhoneNumber.Contains(phoneNumber)).ToListAsync();
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
           await _context.SaveChangesAsync();

            return true;
        }

        return false;

    }

    public async Task<Manufacturer> GetManufactureProductAsync(Guid manufacturerId)
    {
        return (await _context.Manufacturers
            .Include(p => p.Products)
            .ThenInclude(mp => mp.Product)
            .FirstOrDefaultAsync(m => m.Id == manufacturerId))!;
    }
}