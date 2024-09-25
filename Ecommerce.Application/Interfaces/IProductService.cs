using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<ProductDto> AddProductAsync(AddProductDto productDto);
    Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto);
    Task<bool> DeleteProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductAsync();
    Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(String name);
    
    
}