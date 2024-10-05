namespace Ecommerce.Application.DTOs;

public class AddUpdateInvoiceDto
{
    public string OwnerName { get; set; }
    
    public string OwnerFamilyName { get; set; }
    
    public string IdentificationCode { get; set; }
    
    public string IssuerName { get; set; }
    
    public DateTime IssueDate { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string PaymentStatus { get; set; } = "Pending";

}