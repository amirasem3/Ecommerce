namespace Ecommerce.Application.DTOs;

public class ProductInvoiceDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Count { get; set; }
}