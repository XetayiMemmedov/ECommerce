using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public List<OrderItem>? Items { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; }
    public StatusType OrderStatus { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderDto()
    {
        UpdateStatusBasedOnDays();
    }
    public void UpdateStatusBasedOnDays()
    {
        var daysPassed = (DateTime.Now - OrderDate).Days;

        if (daysPassed >= 10)
        {
            OrderStatus = StatusType.Delivered;
        }
        else if (daysPassed >= 3)
        {
            OrderStatus = StatusType.Ontheway;
        }
        else if (daysPassed >= 1)
        {
            OrderStatus = StatusType.Confirmed;
        }
        else
        {
            OrderStatus = StatusType.Pending;
        }
    }
}

public class OrderCreateDto
{
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public int UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public int Quantity { get; set; }
    public StatusType OrderStatus { get; set; }= StatusType.Pending;
    public DateTime DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
   
    
}

public class OrderUpdateDto
{
    public int Id { get; set; }
    public StatusType OrderStatus { get; set; }
}
