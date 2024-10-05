using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IInvoiceServices
{
    public Task<InvoiceDto> GetInvoiceByIdAsync(Guid id);

    public Task<InvoiceDto> GetInvoiceByIdentificationCodeAsync(string identificationCode);
    public Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerNameAsync(string ownerName);

    public Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerFamilyNameAsync(string familyName);

    public Task<IEnumerable<InvoiceDto>> GetInvoicesByIssuerNameAsync(string issuerName);

    public Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentStatusAsync(string paymentStatus);

    public Task<IEnumerable<InvoiceDto>> GetInvoiceByIssueDateAsync(DateTime issueDate);

    public Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentDate(DateTime paymentDate);

    public Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();

    public Task<InvoiceDto> AddInvoiceAsync(AddUpdateInvoiceDto addInvoiceDto);

    public Task<InvoiceDto> UpdateInvoiceAsync(Guid id,UpdateInvoiceDto updateInvoiceDto);

    public Task<bool> DeleteInvoiceAsync(Guid id);

    public Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId);

    public Task<bool> PayAsync(Guid id, decimal price);

    public Task<decimal> CalculateTotalPriceAsync(Guid invoiceId);

    public Task<InvoiceDto> GetInvoiceProductAsync(Guid id);

    public Task AssignInvoiceProductAsync(Guid invoiceId, Guid productId, int count);
    
}