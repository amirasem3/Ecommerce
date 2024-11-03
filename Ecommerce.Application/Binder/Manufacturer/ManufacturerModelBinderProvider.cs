using Ecommerce.Application.Binder.User;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder;

public class ManufacturerModelBinderProvider : IModelBinderProvider
{
    private readonly ManufacturerService _manufacturerService;

    public ManufacturerModelBinderProvider(ManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddUpdateManufacturerDto))
        {
            return new ManufacturerModelBinder(_manufacturerService);
        }

        return null;
    }
}