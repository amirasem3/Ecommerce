namespace Ecommerce.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public decimal Inventory { get; set; }
    
    public DateTime DOP { get; set; }
    
    public DateTime DOE { get; set; }
    
    public bool Status { get; set; }
    
}