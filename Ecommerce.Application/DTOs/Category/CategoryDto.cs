namespace Ecommerce.Application.DTOs;

public class CategoryDto
{


    public CategoryDto(bool categoryType)
    {
        Type = categoryType ? "Parent" : "Child";
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ParentCategoryName { get; set; } 

    public string Type { get; set; }
    
    public Guid ParentCategoryId { get; set; }

    public ICollection<Core.Entities.Category> SubCategories { get; set; } = new List<Core.Entities.Category>();

}