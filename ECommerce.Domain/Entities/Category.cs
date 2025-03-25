namespace ECommerce.Domain.Entities;

public class Category : Entity
{
    public string Name { get; set; } = null!;
    public List<Product>? Products { get; set; } = new List<Product>();
}

