using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder.Invoice;

public class InvoiceModelBinderProvider : IModelBinderProvider
{

    private readonly InvoiceServices _invoiceServices;

    public InvoiceModelBinderProvider(InvoiceServices invoiceServices)
    {
        _invoiceServices = invoiceServices;
    }
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddInvoiceDto) || 
            context.Metadata.ModelType == typeof(UpdateInvoiceDto))
        {
            return new InvoiceModelBinder(_invoiceServices);
        }

        return null;
    }
}