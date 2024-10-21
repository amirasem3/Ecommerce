using System.Collections;
using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<ProductDto> AddProductAsync(AddUpdateProductDto updateProductDto);
    Task<ProductDto> UpdateProductAsync(Guid id, AddUpdateProductDto updateProductDto);
    Task<bool> DeleteProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductAsync();
    Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(string name);

    Task<IEnumerable<ProductDto>> FilterProductByPriceAsync(decimal startPrice, decimal endPrice);
    Task<IEnumerable<InvoiceProductDto>> GetInvoicesByProductId(Guid productId);
}