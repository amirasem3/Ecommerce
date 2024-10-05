namespace Ecommerce.Application.DTOs;

public class AddUpdateCategoryDto
{
    public string CategoryName { get; set; }
    public string ParentCategoryName { get; set; }
    
    public bool Type { get; set; }
    
}