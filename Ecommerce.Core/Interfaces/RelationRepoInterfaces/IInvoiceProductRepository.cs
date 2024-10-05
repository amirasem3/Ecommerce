namespace Ecommerce.Core.Interfaces.RelationRepoInterfaces;

public interface IInvoiceProductRepository
{
    public Task AddInvoiceProductAsync(ProductInvoice productInvoice);

    public Task<bool> DeleteInvoiceProductAsync(Guid  invoiceId, Guid productId);

    public Task<bool> InvoiceHaveTheProductAsync(Guid invoiceId, Guid productId);
}