using Ecommerce.Core.Entities;

namespace Ecommerce.Application.DTOs;

public class AddInvoiceDto
{
    public string OwnerName { get; set; }
    
    public string OwnerFamilyName { get; set; }
    
    public string IdentificationCode { get; set; }
    
    public string IssuerName { get; set; }
    
    public DateTime IssueDate { get; set; }
    

    public decimal TotalPrice { get; set; }
    

}