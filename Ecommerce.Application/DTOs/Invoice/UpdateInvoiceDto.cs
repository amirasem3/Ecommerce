namespace Ecommerce.Application.DTOs;

public class UpdateInvoiceDto
{
    public string OwnerName { get; set; }
    
    public string OwnerFamilyName { get; set; }
    
    
    public DateTime PaymentDate { get; set; }

    public decimal TotalPrice { get; set; }

    //Payment Status: Payed, Pending, Cencelled
    public string PaymentStatus { get; set; }
}