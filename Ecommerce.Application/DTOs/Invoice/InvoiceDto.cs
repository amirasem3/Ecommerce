using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Application.DTOs;

public class InvoiceDto
{
    public Guid Id { get; set; }
    
    public string OwnerName { get; set; }
    
    public string OwnerFamilyName { get; set; }
    
    public string IdentificationCode { get; set; }
    
    public string IssuerName { get; set; }
    
    public DateTime IssueDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }

    public decimal TotalPrice { get; set; }

    //Payment Status: Payed, Pending, Cencelled
    public PaymentStatus PaymentStatus { get; set; }
    
    public ICollection<ProductInvoice> ProductInvoices { get; set; }
}