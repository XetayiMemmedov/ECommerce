using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public class OrderItemDto
    {
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }


    }
    public class OrderItemCreateDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
