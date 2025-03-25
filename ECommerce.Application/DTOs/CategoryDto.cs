namespace ECommerce.Application.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class CategoryCreateDto
{
    public required string Name { get; set; }
}

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}


