using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.Product;

public class ProductModelBinderProvider : IModelBinderProvider
{

    private readonly ProductService _productService;

    public ProductModelBinderProvider(ProductService productService)
    {
        _productService = productService;
    }
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateProductDto))
        {
            return new ProductModelBinder(_productService);
        }

        return null;
    }
}