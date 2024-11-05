using System;

namespace Ecommerce.Application.DTOs;

public class ProductManufacturerDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    
    public decimal Inventory { get; set; }
    
    public DateTime Dop { get; set; }
    
    public DateTime Doe { get; set; }
    
    public bool Status { get; set; }
}