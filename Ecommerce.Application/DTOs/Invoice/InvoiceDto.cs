using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Application.DTOs;

public class InvoiceDto
{
    public Guid Id { get; set; }

    public string? OwnerName { get; init; } = null!;

    public string? OwnerFamilyName { get; init; } = null!;

    public string? IdentificationCode { get; init; } = null!;
    
    public string? IssuerName { get; init; }
    
    public DateTime IssueDate { get; init; }
    
    public DateTime? PaymentDate { get; init; }

    public decimal TotalPrice { get; init; }

    //Payment Status: Payed, Pending, Cencelled
    public PaymentStatus PaymentStatus { get; init; }

    public ICollection<ProductInvoice> ProductInvoices { get; init; } = new List<ProductInvoice>();
}