namespace Ecommerce.Application.DTOs;

public class UpdateInvoiceDto
{
    public string? OwnerName { get; set; }
    
    public string? OwnerFamilyName { get; set; }
    
    public decimal TotalPrice { get; set; }
    
}