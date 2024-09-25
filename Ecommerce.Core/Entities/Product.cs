namespace Ecommerce.Core.Entities;

public class Product
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public decimal Price { get; set; }

    public Product()
    {
        Id = GenerateUniqueId();
    }
    
    private Guid GenerateUniqueId()
    {
        return Guid.NewGuid();
    }
}