using ECommerce.UI;

namespace ECommerce.Domain.Entities;
public class Order : Entity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; }
    public StatusType OrderStatus { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }

}

