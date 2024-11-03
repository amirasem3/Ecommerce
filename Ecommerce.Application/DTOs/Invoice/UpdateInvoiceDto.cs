namespace Ecommerce.Application.DTOs.Invoice;

public class UpdateInvoiceDto
{
    public string OwnerName { get; set; } = null!;

    public string OwnerFamilyName { get; set; } = null!;
    
    public decimal TotalPrice { get; set; }
    
}