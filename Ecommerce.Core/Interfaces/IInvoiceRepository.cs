using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces;

public interface IInvoiceRepository
{
    public Task<Invoice> GetInvoiceById(Guid id);
    public Task<Invoice> GetInvoiceByIdentificationCode(string identificationCode);
    public Task<IEnumerable<Invoice>> SearchInvoicesByOwnerName(string name);

    public Task<IEnumerable<Invoice>> SearchInvoicesByOwnerFamilyName(string familyName);

    public Task<IEnumerable<Invoice>> SearchInvoicesByIssuerName(string issuerName);

    public Task<IEnumerable<Invoice>> SearchInvoicesByPaymentStatus(string paymentStatus);

    public Task<IEnumerable<Invoice>> SearchInvoicesByIssueDate(DateTime issueDate);

    public Task<IEnumerable<Invoice>> SearchInvoicesByPaymentDate(DateTime paymentDate);

    public Task<IEnumerable<Invoice>> GetAllInvoices();

    public Task AddInvoiceAsync(Invoice newInvoice);
    
    public Task UpdateInvoiceAsync(Invoice updateInvoice);

    public Task<bool> DeleteInvoiceAsync(Guid id);

    // public Task<Invoice> GetInvoiceProductAsync(Guid invoiceId);
}