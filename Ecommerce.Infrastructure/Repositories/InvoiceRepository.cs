using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class InvoiceRepository:IInvoiceRepository
{
    private readonly EcommerceDbContext _context;

    public InvoiceRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task<Invoice> GetInvoiceById(Guid id)
    {
        return await _context.Invoices.FindAsync(id);
    }

    public async Task<Invoice> GetInvoiceByIdentificationCode(string identificationCode)
    {
        return (await _context.Invoices.FirstOrDefaultAsync(i => i.IdentificationCode == identificationCode))!;
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByOwnerName(string name)
    {
        return await _context.Invoices.Where(i => i.OwnerName.Contains(name)).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByOwnerFamilyName(string familyName)
    {
        return await _context.Invoices.Where(i => i.OwnerFamilyName.Contains(familyName)).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByIssuerName(string issuerName)
    {
        return await _context.Invoices.Where(i => i.IssuerName.Contains(issuerName)).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByPaymentStatus(string paymentStatus)
    {
        return await _context.Invoices.Where(i => i.PaymentStatus == paymentStatus).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByIssueDate(DateTime issueDate)
    {
        return await _context.Invoices.Where(i => i.IssueDate == issueDate).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> SearchInvoicesByPaymentDate(DateTime paymentDate)
    {
        return await _context.Invoices.Where(i => i.PaymentDate == paymentDate).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetAllInvoices()
    {
        return await _context.Invoices.ToListAsync();
    }

    public async Task AddInvoiceAsync(Invoice newInvoice)
    {
        await _context.Invoices.AddAsync(newInvoice);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInvoiceAsync(Invoice updateInvoice)
    {
        _context.Invoices.Update(updateInvoice);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice!=null)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Invoice> GetInvoiceProductAsync(Guid invoiceId)
    {
        return (await _context.Invoices
            .Include(p => p.Products)
            .ThenInclude(pi => pi.Product)
            .FirstOrDefaultAsync(i => i.Id == invoiceId))!;
    }
}