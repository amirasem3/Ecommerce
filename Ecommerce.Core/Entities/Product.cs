namespace Ecommerce.Core.Entities;

public class Product
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public decimal Price { get; set; }
    
    public decimal Inventory { get; set; }
    
    public DateTime DOP { get; set; }
    
    public DateTime DOE { get; set; }
    
    public bool Status { get; set; }

    public Product()
    {
        Id = GenerateUniqueId();
    }
    
    private Guid GenerateUniqueId()
    {
        return Guid.NewGuid();
    }
}