using Ecommerce.Core.Entities;

namespace Ecommerce.Application.DTOs;

public class AddInvoiceDto
{
    public string? OwnerName { get; set; } = null!;

    public string? OwnerFamilyName { get; set; } = null!;

    public string? IdentificationCode { get; set; } = null!;

    public string? IssuerName { get; set; } = null!;
    
    public DateTime IssueDate { get; set; }
    

    public decimal TotalPrice { get; set; }
    

}