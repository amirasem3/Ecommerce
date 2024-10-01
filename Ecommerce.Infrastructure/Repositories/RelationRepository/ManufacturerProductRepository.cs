using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories.RelationRepository;

public class ManufacturerProductRepository : IManufacturerProductRepository
{

    private readonly EcommerceDbContext _context;

    public ManufacturerProductRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task AddManufacturerProductAsync(ManufacturerProduct manufacturerProduct)
    {
        await _context.ManufacturerProducts.AddAsync(manufacturerProduct);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId)
    {
        var result = await ManufacturerHaveTheProductAsync(manufacturerId, productId);
        if (result)
        {
            Console.WriteLine("Here in If in DeleteManufactureProduct in ManufacturerProductRepository");
            var manufacturerProduct = new ManufacturerProduct
            {
                ManufacturerId = manufacturerId,
                ProductId = productId
            };
            _context.ManufacturerProducts.Remove(manufacturerProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;

    }

    public async Task<bool> ManufacturerHaveTheProductAsync(Guid manufacturerId, Guid productId)
    {
        return await _context.ManufacturerProducts.AnyAsync(mp => mp.ManufacturerId == manufacturerId && mp.ProductId == productId);
    }
}