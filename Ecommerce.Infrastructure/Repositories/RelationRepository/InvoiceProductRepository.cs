using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories.RelationRepository;

public class InvoiceProductRepository : IInvoiceProductRepository
{
    private readonly EcommerceDbContext _context;

    public InvoiceProductRepository(EcommerceDbContext context)
    {
        _context = context;
    }


    public async Task AddInvoiceProductAsync(ProductInvoice productInvoice)
    {
        await _context.ProductInvoices.AddAsync(productInvoice);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        var result = await InvoiceHaveTheProductAsync(invoiceId, productId);
        if (result)
        {
            Console.WriteLine("Here in If in DeleteManufactureProduct in ManufacturerProductRepository");
            var invoiceProduct = new ProductInvoice()
            {
                InvoiceId = invoiceId,
                ProductId = productId
            };
            _context.ProductInvoices.Remove(invoiceProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> InvoiceHaveTheProductAsync(Guid invoiceId, Guid productId)
    {
        return await _context.ProductInvoices.AnyAsync(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
    }
}