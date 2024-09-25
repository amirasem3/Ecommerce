using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetAllProductsByNameAsync(String name);
    Task<Product> AddProductAsync(Product product);
    Task DeleteProductByIdAsync(Guid id);
    Task UpdateProductAsync(Product newProduct);
}