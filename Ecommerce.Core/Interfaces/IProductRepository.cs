using System.Collections;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name);
    Task<Product> AddProductAsync(Product product);
    Task DeleteProductByIdAsync(Guid id);
    Task UpdateProductAsync(Product newProduct);

    Task<IEnumerable<Product>> FilterProductsByPrice(decimal startPrice, decimal endPrice);

    Task<Product> GetProductManufacturersAsync(Guid productId);

    Task<Product> GetProductInvoicesAsync(Guid productId);
}