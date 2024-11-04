using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces.RelationRepoInterfaces;

public class ProductInvoice
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public int Count { get; set; }

    public bool CheckCount()
    {
        return Count > 1;
    }
}