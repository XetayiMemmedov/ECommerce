using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface IOrderService:ICrudService<Order,OrderDto,OrderCreateDto,OrderUpdateDto>
{
    
}
