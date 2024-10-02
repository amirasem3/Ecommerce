using Ecommerce.Core.Entities;

namespace Ecommerce.Application.DTOs;

public class CategoryDto
{
    
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public string ParentCategoryName { get; set; }

    public bool Type { get; set; }
    
    public Guid ParentCategoryId { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

}