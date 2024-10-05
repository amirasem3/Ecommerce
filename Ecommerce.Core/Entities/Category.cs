namespace Ecommerce.Core.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public bool Type { get; set; }
    public Guid ParentCategoryId { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}