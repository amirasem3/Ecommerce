using System.Runtime.InteropServices.JavaScript;

namespace Ecommerce.Application.DTOs;

public class AddUpdateProductDto
{

    public string Name { get; init; } = null!;
    public decimal Price { get; set; }
    public decimal Inventory { get; set; }
    
    public DateTime Dop { get; set; }
    public DateTime Doe { get; set; }
    public bool Status { get; set; }
}