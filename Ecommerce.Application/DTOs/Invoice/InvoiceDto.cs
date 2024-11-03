using Ecommerce.Core.Entities;

namespace Ecommerce.Application.DTOs;

public class InvoiceDto
{
    public Guid Id { get; set; }

    public string OwnerName { get; init; } = null!;

    public string OwnerFamilyName { get; init; } = null!;

    public string IdentificationCode { get; init; } = null!;
    
    public string IssuerName { get; init; } = null!;

    public DateTime IssueDate { get; init; }
    
    public DateTime? PaymentDate { get; init; }

    public decimal TotalPrice { get; init; }
    
    public string PaymentStatus { get; set; } = null!;

    public ICollection<ProductInvoiceDto> Products { get; set; } = new List<ProductInvoiceDto>();
    
    

}