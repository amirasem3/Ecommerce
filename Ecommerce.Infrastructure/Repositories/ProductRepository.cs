using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _context;

    public ProductRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name)
    {

        if (name == " ")
        {
            return await GetAllProductsAsync();
        }

        return await _context.Products.Where(product => product.Name.Contains(name)).ToListAsync();
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product newProduct)
    {
        _context.Products.Update(newProduct);
        await _context.SaveChangesAsync();
    }
    
    
    
}