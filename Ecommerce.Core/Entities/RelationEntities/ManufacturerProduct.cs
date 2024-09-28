namespace Ecommerce.Core.Entities.RelationEntities;

public class ManufacturerProduct
{
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}