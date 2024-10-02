using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Core.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public decimal Inventory { get; set; }
    
    public DateTime DOP { get; set; }
    
    public DateTime DOE { get; set; }
    
    public bool Status { get; set; }

    
    //Relation N-N with Product
    public ICollection<ManufacturerProduct> Manufacturers { get; set; }
}